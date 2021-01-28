import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import Template from './views/Template.vue';
import Partials from './views/Partials.vue';
import Partial from './views/Partial.vue';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      alias: '/templates',
      name: 'templates',
      component: Home,
    },
    {
      path: '/templates/edit/:id',
      name: 'template-edit',
      component: Template,
      props: true,
    },
    {
      path: '/templates/new',
      name: 'template-new',
      component: Template,
    },
    {
      path: '/partials',
      name: 'partials',
      component: Partials,
    },
    {
      path: '/partials/edit/:id',
      name: 'partial-edit',
      component: Partial,
      props: true,
    },
    {
      path: '/partials/new',
      name: 'partial-new',
      component: Partial,
    },
  ],
});
