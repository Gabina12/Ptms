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
          <button :disabled="!partial._id" @click="onDestroy" class="button is-danger">
            <b-icon icon="delete-forever"></b-icon>
            <span>Remove</span>
          </button>
        </div>
        <div class="column">
          <b-field grouped>
            <b-field custom-class="is-small" label="Name" expanded>
              <b-input v-model="partial.name" size="is-small" maxlength="50" hascounter></b-input>
            </b-field>
            <b-field custom-class="is-small" label="Description" expanded>
              <b-input
                v-model="partial.description"
                size="is-small"
                maxlength="50" hascounter></b-input>
            </b-field>
            <b-field custom-class="is-small" label="Category">
              <b-autocomplete
                v-model="partial.category"
                :data="filteredCategories"
                placeholder="e.g. IBank, CRM"
                icon="magnify"
                size="is-small"
                @select="option => partial.category = option">
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
          <div id="template-editor" class="container" @keydown.ctrl.83="onSave($event)" tabindex="0"></div>
        </b-tab-item>
        <b-tab-item icon="settings" label="Settings">
          <div class="container">
            <div class="columns">
              <div class="column">
                <b-field custom-class="is-small" label="ID">
                  <b-tooltip
                       position="is-right"
                       label="This is unique identified of this partial used for rendering">
                    <strong><span>{{></span>{{ partial._id }}<span>}}</span></strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Created">
                  <b-tooltip
                       position="is-right"
                       label="Partial creation date">
                    <strong>{{ new Date(partial.created).toLocaleString('en-US') }}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Creator">
                  <b-tooltip
                       position="is-right"
                       label="User who originally created this partial">
                    <strong>{{partial.creator}}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Updated">
                  <b-tooltip
                       position="is-right"
                       label="Last modified date">
                    <strong>{{ new Date(partial.updated).toLocaleString('en-US') }}</strong>
                  </b-tooltip>
                </b-field>
                <b-field custom-class="is-small" label="Editor">
                  <b-tooltip
                       position="is-right"
                       label="User who edited partial">
                    <strong>{{partial.editor}}</strong>
                  </b-tooltip>
                </b-field>
              </div>
            </div>
          </div>
        </b-tab-item>
      </b-tabs>
    </div>
  </div>
</template>
<style>
#template-editor {
  margin-top: 5px;
  width: 99.9%;
  height: 75vh;
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
import Config from '@/config';
import * as monaco from 'monaco-editor';
import defaultPartial from '@/partial';
import Helpers from '@/helpers';
import NavBar from '@/components/NavBar.vue';

export default {
  name: 'partial-editor',
  props: {
    id: String,
  },
  components: {
    NavBar,
  },
  data() {
    return {
      isLoading: true,
      partial: defaultPartial(),
      editor: null,
      activeTab: 0,
      categories: [],
    };
  },
  computed: {
    filteredCategories() {
      return this.categories.filter(option => option
        .toString()
        .toLowerCase()
        .indexOf(this.partial.category.toLowerCase()) >= 0);
    },
  },
  destroyed() {
    this.editor.dispose();
  },
  mounted() {
    const that = this;
    that.$http.get(`${Config.api}/categories/partials`).then((response) => {
      that.categories = response.body.data;
    }, (response) => {
      Helpers.handleUnauthorized(response, that);
      Helpers.handleInternal(response, that);
      if (Helpers.isBad(response)) {
        that.$toast.open({
          message: 'Cant get partial categories',
          position: 'is-top',
          type: 'is-warning',
        });
      }
      that.$router.push({ name: 'partials' });
    });
    if (!that.id) {
      that.$nextTick(() => {
        that.setupEditor();
        that.isLoading = false;
      });
    } else {
      that.$http.get(`${Config.api}/partials/${that.id}`).then((response) => {
        response.body.data.category = response.body.data.category || '';
        that.partial = response.body.data;
        that.$nextTick(() => {
          that.setupEditor();
          that.isLoading = false;
        });
      }, (response) => {
        Helpers.handleUnauthorized(response, that);
        Helpers.handleInternal(response, that);
        if (Helpers.isBad(response)) {
          that.$toast.open({
            message: 'Id doest exsists or cant get partial',
            position: 'is-top',
            type: 'is-warning',
          });
        }
        that.isLoading = false;
        that.$router.push({ name: 'partials' });
      });
    }
  },
  methods: {
    setupEditor() {
      const that = this;
      if (!that.editor) {
        that.editor = monaco.editor.create(document.getElementById('template-editor'), {
          value: '',
          language: 'html',
          theme: 'vs-dark',
          readOnly: false,
        });
      }
      that.editor.setValue(that.partial.template);
    },
    onDestroy() {
      const that = this;
      Helpers.destroyPartial(this.partial._id, this.partial.name, this, () => that.$router.push({ name: 'partials' }));
    },
    onSave(event) {
      if (event) event.preventDefault();
      const that = this;
      that.isLoading = true;
      that.partial.template = that.editor.getValue();
      const method = that.partial._id ? 'post' : 'put';
      let addr = `${Config.api}/partials`;
      if (that.partial._id) addr = `${addr}/${that.partial._id}`;
      that.$http[method](addr, that.partial).then((response) => {
        that.isLoading = false;
        that.$toast.open({
          message: 'Successfully saved',
          position: 'is-top',
          type: 'is-success',
        });
        if (!that.partial._id) {
          that.partial = defaultPartial();
          that.$router.push({ name: 'partial-edit', params: { id: response.body.data.id } });
          that.$router.go();
        }
      }, (response) => {
        Helpers.handleUnauthorized(response, that);
        Helpers.handleInternal(response, that);
        Helpers.handleLargePayload(response, that);
        if (Helpers.isBad(response)) {
          that.$toast.open({
            message: 'Wrong parameters, cant save template',
            position: 'is-top',
            type: 'is-warning',
          });
        }
        that.isLoading = false;
      });
    },
  },
};
</script>
