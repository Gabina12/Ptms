import Vue from 'vue';
import Buefy from 'buefy';
import JsonExcel from 'vue-json-excel';
import VueResource from 'vue-resource';
import VueCookie from 'vue-cookie';
import 'buefy/dist/buefy.css';
import '@mdi/font/css/materialdesignicons.min.css';
import App from './App.vue';
import router from './router';
import axios from 'axios';

Vue.use(VueResource);
Vue.use(VueCookie);
Vue.use(Buefy);
Vue.component('downloadExcel', JsonExcel)
Vue.config.productionTip = false;

//Vue.http.interceptors.push((request, next) => {
//  request.headers.set('Authorization', Vue.cookie.get('bauth-token'));
//  next();
//});

//axios.interceptors.request.use(
//  (config) => {
//    let token = Vue.cookie.get('bauth-token');

//    if (token) {
//      config.headers['Authorization'] = `${ token }`;
//    }

//    return config;
//  }, 

//  (error) => {
//    return Promise.reject(error);
//  }
//);

new Vue({
  router,
  render: h => h(App),
}).$mount('#app');
