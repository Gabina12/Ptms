<template>
  <div class="container template-list">
    <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
    <button class="button field is-primary" @click="addNew">
      <b-icon icon="plus"></b-icon>
      <span>New Template</span>
    </button>
    <button class="button field is-success" @click="backup">
      <b-icon icon="archive"></b-icon>
      <span>Backup</span>
    </button>
    <b-upload @input="restore" v-model="restoreFile">
      <a class="button field is-success">
        <b-icon icon="upload"></b-icon>
        <span>Restore</span>
      </a>
    </b-upload>
    <button class="button field is-danger" @click="onBulkDestroy">
      <b-icon icon="trash-can"></b-icon>
      <span>Delete Selected</span>
    </button>
    <download-excel
      class="button field is-success"
      :fetch="fetchData"
      :before-generate="startDownload"
      :before-finish="finishDownload"
      type="xls"
    >
      <b-icon icon="export"></b-icon>
      <span>Export</span>
      <!-- <img src="download_icon.png"> -->
    </download-excel>
    <b-collapse
      v-for="category in categories"
      v-bind:data="category"
      v-bind:key="category"
      :open="false"
      style="margin: 5px 0px 5px 0px"
      class="card"
    >
      <div slot="trigger" slot-scope="props" class="card-header">
        <p class="card-header-title">{{category}} ({{catTemplates[category].length}})</p>
        <a class="card-header-icon">
          <b-icon :icon="props.open ? 'menu-down' : 'menu-up'"></b-icon>
        </a>
      </div>
      <div class="card-content">
        <div class="content">
          <b-table
            ref="table"
            default-sort-direction="desc"
            default-sort="updated"
            :data="catTemplates[category]"
            :checked-rows.sync="checkedRows[category]"
            :narrowed="false"
            :hoverable="true"
            detailed
            detail-key="_id"
            class="m-t-10"
            checkable
          >
            <template slot-scope="props">
              <b-table-column width="300" label="ID" centered>
                <strong>{{ props.row._id }}</strong>
              </b-table-column>
              <b-table-column label="Name" field="name" sortable centered>{{ props.row.name }}</b-table-column>
              <!-- <b-table-column
                label="Description"
                field="description"
                sortable
                centered>
                {{ props.row.description }}
              </b-table-column>-->
              <b-table-column
                label="Last Updated"
                field="updated"
                sortable
                centered
              >{{ new Date(props.row.updated).toLocaleString('en-US') }}</b-table-column>
                <b-table-column label="Version" field="version" sortable centered>
                  <select @change="handleversionchange($event, props.row._id)">
                    <option v-for="version in props.row.versions" >{{version}}</option>
                  </select>
                </b-table-column>
              <b-table-column label="Editor" field="name" sortable centered>{{ props.row.editor }}</b-table-column>
              <b-table-column label="Action" centered>
                <b-tooltip class="is-success" label="Edit template">
                  <button @click="edit(props.row._id)" class="button is-small is-success">
                    <b-icon icon="pencil" size="is-small"></b-icon>
                  </button>
                </b-tooltip>
                <b-tooltip class="is-primary" label="Preview template HTML">
                  <button @click="onTxtPreview(props.row._id, props.row.version)" class="button is-small is-primary">
                    <b-icon icon="eye" size="is-small"></b-icon>
                  </button>
                </b-tooltip>
                <b-tooltip class="is-primary" label="Preview template PDF">
                  <button @click="onPdfPreview(props.row._id, props.row.version)" class="button is-small is-primary">
                    <b-icon icon="file" size="is-small"></b-icon>
                  </button>
                </b-tooltip>
                <b-tooltip class="is-danger" label="Delete template">
                  <button
                    class="button is-small is-danger"
                    @click="onDestroy(props.row._id, props.row.name)"
                  >
                    <b-icon icon="delete-forever" size="is-small"></b-icon>
                  </button>
                </b-tooltip>
              </b-table-column>
            </template>

            <template slot="detail" slot-scope="props">
              <article class="media">
                <div class="media-content">
                  <div class="content">
                    <p>
                      <strong>Description:</strong>
                    </p>
                    <p>{{ props.row.description }}</p>
                  </div>
                </div>
              </article>
            </template>
            <template slot="empty">
              <section class="section">
                <div class="content has-text-grey has-text-centered">
                  <p>
                    <b-icon icon="emoticon-sad" size="is-large"></b-icon>
                  </p>
                  <p>No Templates</p>
                </div>
              </section>
            </template>
          </b-table>
        </div>
      </div>
    </b-collapse>
  </div>
</template>

<script>
import Config from "@/config";
import Helpers from "@/helpers";
import Backup from "@/backup";
import axios from "axios";

export default {
  name: "TemplateList",
  props: {
    templates: Array,
    onDelete: Function,
    onSelectedVersion:Function
  },
  data() {
    return {
      isLoading: false,
      restoreFile: null,
      checkedRows: {}
    };
  },
  computed: {
    categories() {
      const categories = this.templates
        .map(elem => elem.category)
        .filter(Helpers.onlyUnique)
        .filter(elem => !!elem && elem.trim());
      if (categories.indexOf("Uncategorized") == -1)
        categories.push("Uncategorized");
      return categories;
    },
    catTemplates() {
      const map = {};
      this.categories.forEach(category => {
        map[category] = this.templates.filter(
          elem => elem.category === category
        );
      });
      map["Uncategorized"] = map["Uncategorized"].concat(
        this.templates.filter(elem => !elem.category || !elem.category.trim())
      );
      return map;
    }
  },
  methods: {
    async fetchData() {
      const that = this;
      const response = await axios.get(`${Config.api}/export`);
      debugger;
      const templateList = Object.keys(that.checkedRows).reduce(
        (list, k) => list.concat(that.checkedRows[k]),
        []
      );

      let exportdata = templateList.length == 0 ? response.data.data : [];
      if (exportdata.length == 0) {
        response.data.data.forEach(element => {
          if (templateList.filter(x => x._id == element._id).length == 1) {
            exportdata.push({
              id: element._id,
              name: element.name,
              description: element.description,
              category: element.category,
              created: new Date(element.created),
              updated: new Date(element.updated),
              creator: element.creator,
              editor: element.editor,
              templateTxt: element.templateTxt
            });
          }
        });
      }
      return exportdata;
    },
    startDownload() {
      //alert('show loading');
    },
    finishDownload() {
      //alert('hide loading');
    },
    addNew() {
      this.$router.push({ name: "template-new" });
    },
    onPdfPreview(id, version) {
      Helpers.previewTemplatePdf(id, version, this);
    },
    onTxtPreview(id, version) {
      Helpers.previewTemplateTxt(id,version, this);
    },
    onDestroy(id, name) {
      const that = this;
      Helpers.destroyTemplate(id, name, that, that.onDelete);
    },
    onBulkDestroy() {
      const that = this;
      const templateList = Object.keys(that.checkedRows).reduce(
        (list, k) => list.concat(that.checkedRows[k]),
        []
      );
      Helpers.destroyTemplateBulk(templateList, that, that.onDelete);
    },
    edit(id) {
      const that = this;
      that.$router.push({ name: "template-edit", params: { id } });
    },
    backup() {
      Backup.backupTemplates(this);
    },
    restore(restore) {
      Backup.restoreTemplates(restore, this);
    },
    handleversionchange(e, id){
        const that = this;        
        that.onSelectedVersion(id,e.target.value);
    },
  }
};
</script>

<style scoped>
.template-list {
  margin-top: 20px;
  margin-bottom: 20px;
}
button.is-small {
  margin: 2px;
}
</style>
