<template>
  <div class="container partial-list">
    <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
    <button class="button field is-primary" @click="addNew">
      <b-icon icon="plus"></b-icon>
      <span>New Partial</span>
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
    <b-collapse
      v-for="category in categories"
      v-bind:data="category"
      v-bind:key="category"
      :open="false"
      style="margin: 5px 0px 5px 0px"
      class="card">
      <div slot="trigger" slot-scope="props" class="card-header">
        <p class="card-header-title">
          {{category}} ({{catPartials[category].length}})
        </p>
        <a class="card-header-icon">
          <b-icon
            :icon="props.open ? 'menu-down' : 'menu-up'">
          </b-icon>
        </a>
      </div>
      <div class="card-content">
        <div class="content">
          <b-table
             default-sort="updated"
             default-sort-direction="desc"
             :data="catPartials[category]"
             :checked-rows.sync="checkedRows[category]"
             checkable>
            <template slot-scope="props">
              <b-table-column width="300" label="ID" centered>
                <strong><span>{{></span>{{ props.row._id }}<span>}}</span></strong>
              </b-table-column>
              <b-table-column
                label="Name"
                field="name"
                sortable
                centered>
                {{ props.row.name }}
              </b-table-column>
              <b-table-column
                label="Description"
                field="description"
                sortable
                centered>
                {{ props.row.description }}
              </b-table-column>
              <b-table-column
                label="Last Updated"
                field="updated"
                sortable
                centered>
                {{ new Date(props.row.updated).toLocaleString('en-US') }}
              </b-table-column>
              <b-table-column
                label="Editor"
                field="editor"
                sortable
                centered>
                {{ props.row.editor }}
              </b-table-column>
              <b-table-column label="Action" centered>
                <b-tooltip class="is-success" label="Edit partial">
                  <button @click="edit(props.row._id)" class="button is-small is-success">
                    <b-icon
                      icon="pencil"
                      size="is-small">
                    </b-icon>
                  </button>
                </b-tooltip>
                <b-tooltip class="is-danger" label="Delete partial">
                  <button class="button is-small is-danger"
                          @click="onDestroy(props.row._id, props.row.name)">
                    <b-icon
                      icon="delete-forever"
                      size="is-small">
                    </b-icon>
                  </button>
                </b-tooltip>
              </b-table-column>
            </template>
            <template slot="empty">
              <section class="section">
                <div class="content has-text-grey has-text-centered">
                  <p>
                    <b-icon
                      icon="emoticon-sad"
                      size="is-large">
                    </b-icon>
                  </p>
                  <p>No Partials</p>
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
import Helpers from '@/helpers';
import Backup from '@/backup';

export default {
  name: 'PartialList',
  props: {
    partials: Array,
    onDelete: Function,
  },
  data() {
    return {
      isLoading: false,
      restoreFile: null,
      checkedRows: {},
    };
  },
  computed: {
    categories() {
      const categories = this.partials
        .map(elem => elem.category)
        .filter(Helpers.onlyUnique)
        .filter(elem => !!elem && elem.trim());
      if (categories.indexOf('Uncategorized') == -1)
        categories.push('Uncategorized');
      return categories;
    },
    catPartials() {
      const map = {};
      this.categories.forEach((category) => {
        map[category] = this.partials
          .filter(elem => elem.category === category);
      });
      map['Uncategorized'] = map['Uncategorized'].concat(this.partials.filter(elem => !elem.category || !elem.category.trim()));
      return map;    
    },
  },
  methods: {
    addNew() {
      this.$router.push({ name: 'partial-new' });
    },
    onDestroy(id, name) {
      const that = this;
      Helpers.destroyPartial(id, name, that, that.onDelete);
    },
    onBulkDestroy() {
      const that = this;
      const partialList = Object.keys(that.checkedRows).reduce((list, k) => list.concat(that.checkedRows[k]), []);
      Helpers.destroyPartialBulk(partialList, that, that.onDelete);
    },
    edit(id) {
      const that = this;
      that.$router.push({ name: 'partial-edit', params: { id } });
    },
    backup() {
      Backup.backupPartials(this);
    },
    restore(file) {
      Backup.restorePartials(file, this);
    },
  },
};
</script>

<style scoped>
.partial-list {
  margin-top: 20px;
  margin-bottom: 20px;
}
button.is-small {
  margin: 2px;
}
</style>
