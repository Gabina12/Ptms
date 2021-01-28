<template>
  <div>
    <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
    <NavBar></NavBar>
    <div class="container">
      <div class="columns">
        <div class="column is-half">
          <button @click="onSave" class="button is-success">
            <b-icon icon="content-save"></b-icon>
            <span>Save</span>
          </button>
          <button :disabled="!template._id" @click="onTxtPreview" class="button is-primary">
            <b-icon icon="eye"></b-icon>
            <span>Html</span>
          </button>
          <button :disabled="!template._id" @click="onPdfPreview" class="button is-primary">
            <b-icon icon="file"></b-icon>
            <span>PDF</span>
          </button>
          <button :disabled="!template._id" @click="onDestroy" class="button is-danger">
            <b-icon icon="delete-forever"></b-icon>
            <span>Remove</span>
          </button>
          <button :disabled="!template._id" @click="onUpgrade" class="button is-success">
            <b-icon icon="arrow-up"></b-icon>
            <span>Change Version</span>
          </button>
        </div>
        <div class="column">
          <b-field grouped>
            <b-field custom-class="is-small" label="Version" expanded>
              <b-input v-model="template.version" size="is-small" type="text" disabled></b-input>
            </b-field>
            <b-field custom-class="is-small" label="Name" expanded>
              <b-input v-model="template.name" size="is-small" maxlength="50" hascounter></b-input>
            </b-field>
            <b-field custom-class="is-small" label="Description" expanded>
              <b-input v-model="template.description" size="is-small" maxlength="1500" hascounter></b-input>
            </b-field>
            <b-field custom-class="is-small" label="Category">
              <b-autocomplete
                v-model="template.category"
                :data="filteredCategories"
                placeholder="e.g. IBank, CRM"
                icon="magnify"
                size="is-small"
                @select="option => template.category = option"
              >
                <template slot="empty">No categories found</template>
              </b-autocomplete>
            </b-field>
          </b-field>
        </div>
      </div>
    </div>
    <div class="container">
      <b-tabs v-model="activeTab" position="is-centered" class="block">
        <b-tab-item icon="pencil" label="Editor">
          <div
            id="template-editor"
            class="container"
            @keydown.ctrl.83="onSave($event)"
            tabindex="0"
          ></div>
        </b-tab-item>
        <b-tab-item icon="settings" label="Settings">
          <div class="container">
            <div class="columns">
              <div class="column">
                <b-field custom-class="is-small" label="ID">
                  <b-tooltip
                    position="is-right"
                    label="This is unique identified of this template used for rendering"
                  >
                    <strong>{{template._id}}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Created">
                  <b-tooltip position="is-right" label="Template creation date">
                    <strong>{{ new Date(template.created).toString() }}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Creator">
                  <b-tooltip position="is-right" label="User who originally created this template">
                    <strong>{{template.creator}}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Updated">
                  <b-tooltip position="is-right" label="Last modified date">
                    <strong>{{ new Date(template.updated).toString() }}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Editor">
                  <b-tooltip position="is-right" label="User who edited template">
                    <strong>{{template.editor}}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Scale">
                  <b-tooltip
                    position="is-right"
                    label="Scale of the document, be carefoul, this must be double, positive"
                  >
                    <b-input v-model="template.options.scale" size="is-small"></b-input>
                  </b-tooltip>
                </b-field>
                <b-field>
                  <b-field custom-class="is-small" label="Top margin">
                    <b-input v-model="template.options.margin.top" size="is-small"></b-input>
                  </b-field>
                  <b-field custom-class="is-small" label="Bottom margin">
                    <b-input v-model="template.options.margin.bottom" size="is-small"></b-input>
                  </b-field>
                </b-field>
                <b-field>
                  <b-field custom-class="is-small" label="Right margin">
                    <b-input v-model="template.options.margin.right" size="is-small"></b-input>
                  </b-field>
                  <b-field custom-class="is-small" label="Left margin">
                    <b-input v-model="template.options.margin.left" size="is-small"></b-input>
                  </b-field>
                </b-field>
              </div>
              <div class="column">
                <b-field custom-class="is-small" label="External sources">
                  <b-tooltip
                    position="is-right"
                    label="Renderer will wait for resources to be loaded, slows speed"
                  >
                    <b-switch
                      v-model="template.loadExternalSources"
                    >{{ template.loadExternalSources ? 'Enabled' : 'Disabled' }}</b-switch>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Display header/footer">
                  <b-tooltip
                    position="is-right"
                    label="Use header/footer templates to display info top/bottom"
                  >
                    <b-switch
                      v-model="template.options.displayHeaderFooter"
                    >{{ template.options.displayHeaderFooter ? 'Enabled' : 'Disabled' }}</b-switch>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Header template">
                  <b-tooltip
                    position="is-right"
                    label="Use css classes date,title,url,pageNumber,totalPages to inject data"
                  >
                    <b-input
                      v-model="template.options.headerTemplate"
                      size="is-small"
                      type="textarea"
                    ></b-input>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Footer template">
                  <b-tooltip
                    position="is-right"
                    label="Use css classes date,title,url,pageNumber,totalPages to inject data"
                  >
                    <b-input
                      v-model="template.options.footerTemplate"
                      size="is-small"
                      type="textarea"
                    ></b-input>
                  </b-tooltip>
                </b-field>
              </div>
              <div class="column">
                <b-field custom-class="is-small" label="Caching">
                  <b-tooltip position="is-right" label="File is cached for every different input">
                    <b-switch
                      v-model="template.caching"
                    >{{ template.caching ? 'Enabled' : 'Disabled' }}</b-switch>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Background images">
                  <b-tooltip
                    position="is-top"
                    label="Check this if you want background graphics enabled"
                  >
                    <b-switch
                      v-model="template.options.printBackground"
                    >{{ template.options.printBackground ? 'Enabled' : 'Disabled' }}</b-switch>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Orientation">
                  <b-tooltip position="is-top" label="Orientation of document">
                    <b-switch
                      v-model="template.options.landscape"
                    >{{ template.options.landscape ? 'Landscape' : 'Portrait' }}</b-switch>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Document size">
                  <b-tooltip position="is-top" label="Document size is various formats">
                    <b-select v-model="template.options.format">
                      <option value="Letter">Letter: 8.5in x 11in</option>
                      <option value="Legal">Legal: 8.5in x 14in</option>
                      <option value="Tabloid">Tabloid: 11in x 17in</option>
                      <option value="Ledger">Ledger: 17in x 11in</option>
                      <option value="A0">A0: 33.1in x 46.8in</option>
                      <option value="A1">A1: 23.4in x 33.1in</option>
                      <option value="A2">A2: 16.5in x 23.4in</option>
                      <option value="A3">A3: 11.7in x 16.5in</option>
                      <option value="A4">A4: 8.27in x 11.7in</option>
                      <option value="A5">A5: 5.83in x 8.27in</option>
                      <option value="A6">A6: 4.13in x 5.83in</option>
                    </b-select>
                  </b-tooltip>
                </b-field>
              </div>
            </div>
          </div>
        </b-tab-item>

        <b-tab-item icon="history" label="Version History">
          <div class="container">
             <b-table
            ref="table"
            default-sort-direction="desc"
            default-sort="updated"
            :data="template.versionControl"
            :narrowed="false"
            :hoverable="true"
            class="m-t-10"
          >
           <template slot-scope="props">
             
              <b-table-column label="Version" field="version" sortable centered>{{ props.row.version }}</b-table-column>
              <b-table-column label="Created" field="created" sortable centered>{{ new Date(props.row.created).toLocaleString('en-US') }}</b-table-column>
              <b-table-column label="Creator" field="creator" sortable centered>{{ props.row.creator }}</b-table-column>

              <b-table-column label="Action" centered>
                <b-tooltip class="is-primary" label="Preview template HTML" style="margin-right:5px;">
                  <button @click="onTxtPreviewVersion(props.row.version)" class="button is-small is-primary">
                    <b-icon icon="eye" size="is-small"></b-icon>
                  </button>
                </b-tooltip>
                <b-tooltip class="is-primary" label="Preview template PDF">
                  <button @click="onPdfPreviewVersion(props.row.version)" class="button is-small is-primary">
                    <b-icon icon="file" size="is-small"></b-icon>
                  </button>
                </b-tooltip>
              </b-table-column>
           </template>

             </b-table>
             </div>
        </b-tab-item>
      </b-tabs>
    </div>
  </div>
</template>
<style>
#template-editor {
  margin-top: 5px;
  width: 100%;
  height: 80vh;
}

.column button {
  margin: 5px;
}

.field {
  margin: 3px;
}
</style>
<script>
/* eslint no-underscore-dangle: ["error", { "allow": ["_id"] }] */
// @ is an alias to /src
import Config from "@/config";
import * as monaco from "monaco-editor";
import Helpers from "@/helpers";
import NavBar from "@/components/NavBar.vue";
import DefaultTemplate from "@/template";
import { debug } from "util";

export default {
  name: "template-editor",
  props: {
    id: String
  },
  components: {
    NavBar
  },
  data() {
    return {
      isLoading: true,
      template: DefaultTemplate(),
      editor: null,
      activeTab: 0,
      categories: [],
      errors: []
    };
  },
  computed: {
    filteredCategories() {
      return this.categories.filter(
        option =>
          option
            .toString()
            .toLowerCase()
            .indexOf(this.template.category.toLowerCase()) >= 0
      );
    }
  },
  destroyed() {
    this.editor.dispose();
  },
  mounted() {
    const that = this;
    that.$http.get(`${Config.api}/categories/templates`).then(
      response => {
        that.categories = response.body.data;
      },
      response => {
        Helpers.handleUnauthorized(response, that);
        Helpers.handleInternal(response, that);
        if (Helpers.isBad(response)) {
          that.$toast.open({
            message: "Cant get template categories",
            position: "is-top",
            type: "is-warning"
          });
        }
        that.$router.push({ name: "templates" });
      }
    );
    if (!that.id) {
      that.$nextTick(() => {
        that.setupEditor();
        that.template.version = "v1";
        that.isLoading = false;
      });
    } else {
      that.$http.get(`${Config.api}/templates/${that.id}`).then(
        response => {
          response.body.data.category = response.body.data.category || "";
          that.template = response.body.data;
          that.$nextTick(() => {
            that.setupEditor();
            that.isLoading = false;
          });
        },
        response => {
          Helpers.handleUnauthorized(response, that);
          Helpers.handleInternal(response, that);
          if (Helpers.isBad(response)) {
            that.$toast.open({
              message: "Id doest exsists or cant get template",
              position: "is-top",
              type: "is-warning"
            });
          }
          that.isLoading = false;
          that.$router.push({ name: "templates" });
        }
      );
    }
  },
  methods: {
    setupEditor() {
      const that = this;
      if (!that.editor) {
        that.editor = monaco.editor.create(
          document.getElementById("template-editor"),
          {
            value: "",
            language: "html",
            theme: "vs-dark",
            readOnly: false
          }
        );
      }
      that.editor.setValue(that.template.template);
    },
    onPdfPreview() {
      Helpers.previewTemplatePdf(
        this.template._id,
        this.template.version,
        this
      );
    },
    onTxtPreview() {
      Helpers.previewTemplateTxt(
        this.template._id,
        this.template.version,
        this
      );
    },
    onPdfPreviewVersion(version) {
      Helpers.previewTemplatePdf(
        this.template._id,
        version,
        this
      );
    },
    onTxtPreviewVersion(version) {
      Helpers.previewTemplateTxt(
        this.template._id,
        version,
        this
      );
    },
    onDestroy() {
      const that = this;
      Helpers.destroyTemplate(this.template._id, this.template.name, this, () =>
        that.$router.push({ name: "templates" })
      );
    },
    onUpgrade(event) {
      const that = this;
      that.template.template = that.editor.getValue();
      that.template.options.scale = parseInt(that.template.options.scale, 10);
      that.$dialog.confirm({
        title: "Version Change",
        message: `Are you sure you want to <b>change</b> template version?`,
        confirmText: "Yes",
        type: "is-info",
        hasIcon: true,
        onConfirm: () => {
          that.isLoading = true;
          console.log(that.template.version, "ver");
          let oldVersion = that.template.version.substring(1);
          console.log(oldVersion, "old");
          that.isLoading = false;
          that.template.version = "v" + (parseInt(oldVersion) + 1);
          console.log(that.template.version, "ver");
          that.onSave(event);
        }
      });
    },
    onSave(event) {
      if (event) event.preventDefault();
      const that = this;
      that.isLoading = true;
      that.template.template = that.editor.getValue();
      that.template.options.scale = parseInt(that.template.options.scale, 10);
      const method = that.template._id ? "post" : "put";
      let addr = `${Config.api}/templates`;
      if (that.template._id) addr = `${addr}/${that.template._id}`;
      that.$http[method](addr, that.template).then(
        response => {
          that.isLoading = false;
          that.$toast.open({
            message: "Successfully saved",
            position: "is-top",
            type: "is-success"
          });
          // setTimeout(function() {
          //   that.$router.push({ name: "templates" });
          //   //that.$router.go();
          // }, 500);

          if (!that.template._id) {
            that.template = DefaultTemplate();
            that.$router.push({
              name: "template-edit",
              params: { id: response.body.data.id }
            });
            that.$router.go();
          }
        },
        response => {
          debugger;
          Helpers.handleUnauthorized(response, that);
          Helpers.handleInternal(response, that);
          Helpers.handleLargePayload(response, that);
          if (Helpers.isBad(response)) {
            if (response.body.msg == "ALREADY_REGISTERED") {
              that.$toast.open({
                message: "template with this name already exists!",
                position: "is-top",
                type: "is-warning"
              });
            } else {
              that.$toast.open({
                message: "Wrong parameters, cant save template",
                position: "is-top",
                type: "is-warning"
              });
            }
          }
          that.isLoading = false;
        }
      );
    }
  }
};
</script>
