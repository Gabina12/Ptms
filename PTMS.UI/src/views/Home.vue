<template>
  <div class="home">
    <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
    <NavBar></NavBar>
      <div class="container template-list">
        <b-field>
            <b-input placeholder="search by name, description or id..."
                type="search"
                style="width:100%;"
                v-model="query"
                @keyup.enter.native="search"
                icon="magnify">
            </b-input>
            <p class="control" >
                <button class="button is-primary"
                @click="search"
                >Search</button>
            </p>
        </b-field>
      </div>
    <TemplateList :onDelete="onDelete" :templates="templates" :onSelectedVersion="changeSelectedVersion"></TemplateList>
  </div>
</template>

<style>

</style>
<script>
// @ is an alias to /src
import Config from '@/config';
import Helpers from '@/helpers';
import NavBar from '@/components/NavBar.vue';
import TemplateList from '@/components/TemplateList.vue';

export default {
  name: 'home',
  components: {
    NavBar,
    TemplateList,
  },
  data() {
    return {
      isLoading: true,
      templates: [],
      query: ''
    };
  },
  mounted() {
    this.loadTemplates();
  },
  methods: {
    changeSelectedVersion(id,version){
      const that = this;
      let {templates} = that;
      templates = templates.map(item=>{
        if(item._id==id){
          item.selectedVersion = version
        }
        return item;
      })
      that.templates = templates
      
    },
    search() {
    const that = this;
      that.$http.get(`${Config.api}/templates`).then((response) => {
        that.isLoading = false;
        that.templates = response.body.data.filter(x=> x.name.indexOf(that.query) > -1 || x.description.indexOf(that.query) > -1 || x._id == that.query);
        that.templates = that.templates.map(i=>{
          let version = i.version == null ? 1: Number(i.version.substr(1));
          i.versions =  []
          for(let j=version; j>0;j--){            
            i.versions.push(`v${j}`)
          }
          i.selectedVersion = i.version;
          return i
        })
      }, (response) => {
        Helpers.handleUnauthorized(response, that);
        Helpers.handleInternal(response, that);
        that.isLoading = false;
      });
    },
    onDelete() {
      this.loadTemplates();
    },
    loadTemplates() {
      const that = this;
      that.$http.get(`${Config.api}/templates`).then((response) => {
        that.isLoading = false;
        let templates = response.body.data;
       templates =  templates.map(i=>{
          let version = i.version == null ? 1: Number(i.version.substr(1));                    
          i.versions =  []
          for(let j=version; j>0;j--){            
            i.versions.push(`v${j}`)
          }

          i.selectedVersion = i.version;
          return i
        })
        that.templates = templates;
      }, (response) => {
        Helpers.handleUnauthorized(response, that);
        Helpers.handleInternal(response, that);
        that.isLoading = false;
      });
    },
  },
};
</script>
