<template>
  <div>
    <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>
    <NavBar></NavBar>
    <PartialList :onDelete="onDelete" :partials="partials"></PartialList>
  </div>
</template>

<style>

</style>
<script>
// @ is an alias to /src
import Config from '@/config';
import Helpers from '@/helpers';
import NavBar from '@/components/NavBar.vue';
import PartialList from '@/components/PartialList.vue';

export default {
  name: 'partials-list',
  components: {
    NavBar,
    PartialList,
  },
  data() {
    return {
      isLoading: true,
      partials: [],
    };
  },
  mounted() {
    this.loadTemplates();
  },
  methods: {
    onDelete() {
      this.loadTemplates();
    },
    loadTemplates() {
      const that = this;
      that.$http.get(`${Config.api}/partials`).then((response) => {
        that.isLoading = false;
        that.partials = response.body.data;
      }, (response) => {
        Helpers.handleUnauthorized(response, that);
        Helpers.handleInternal(response, that);
        that.isLoading = false;
      });
    },
  },
};
</script>
