<template>
  <div class="container-fluid" style="margin-bottom:20px;">
    <nav class="navbar" style="background:#000;box-shadow: 0 1px 10px rgba(0,0,0,.3);" role="navigation" aria-label="main navigation">
      <div class="container">
      
       <div class="navbar-brand">
    <a class="navbar-item" href="/">
      <img src="https://proway.ge/img/logo-header.svg">
    </a>

    <a role="button" class="navbar-burger burger" aria-label="menu" aria-expanded="false" data-target="navbarBasicExample">
      <span aria-hidden="true"></span>
      <span aria-hidden="true"></span>
      <span aria-hidden="true"></span>
    </a>
  </div>

      <div class="navbar-menu">
        <div class="navbar-end">
          <router-link class="navbar-item" to="/">
            <b-icon
              icon="file-document"
              size="is-small">
            </b-icon>
            <span>Templates</span>
          </router-link>
          <router-link class="navbar-item" to="/partials">
            <b-icon
              icon="file-multiple"
              size="is-small">
            </b-icon>
            <span>Global Partials</span>
          </router-link>
          <!--<router-link class="navbar-item" to="/users">
            <b-icon
              icon="account-multiple"
              size="is-small">
            </b-icon>
            <span>Users</span>
          </router-link>
          <a class="navbar-item" @click="onInfo">
            <b-icon
              icon="help-box"
              size="is-small">
            </b-icon>
            <span>About</span>
          </a>
          <router-link class="navbar-item" to="/logout">
            <b-icon
              icon="logout"
              size="is-small">
            </b-icon>
            <span>Log Out</span>
          </router-link>-->
        </div>
      </div>
      </div>
    </nav>
  </div>
</template>

<script>
import InfoForm from '@/components/InfoForm.vue';

export default {
  name: 'NavBar',
  methods: {
    onInfo() {
      const that = this;
      this.$modal.open({
        parent: that,
        component: InfoForm,
        hasModalCard: true,
      });
    },
  },
};
</script>

<style scoped>
.router-link-exact-active span {
  color: blue;
}
</style>
