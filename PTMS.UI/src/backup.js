/* eslint no-param-reassign: ["error", { "props": false }] */
import async from 'async';
import Config from '@/config';
import Download from 'downloadjs';
import Helpers from '@/helpers';

const Backup = {
  backupTemplates(context) {
    const templates = [];
    // const templateList = response.body.data;
    const templateList = Object.keys(context.checkedRows).reduce((list, k) => list.concat(context.checkedRows[k]), []);
    if (!templateList.length) {
      context.$toast.open({
        message: 'Select templates first!',
        position: 'is-top',
        type: 'is-warning',
      });
      return;
    }
    context.isLoading = true;
    async.each(templateList, (elem, next) => {
      context.$http.get(`${Config.api}/templates/${elem._id}/${elem.selectedVersion}`).then((template) => {
        templates.push(template.body.data);
        next();
      }, (template) => {
        Helpers.handleUnauthorized(template, context);
        Helpers.handleInternal(template, context);
        if (Helpers.isBad(template)) {
          context.$toast.open({
            message: 'Bad request',
            position: 'is-top',
            type: 'is-warning',
          });
        }
      });
    }, () => {
      context.isLoading = false;
      Download(new Blob([JSON.stringify(templates)]), `templates-backup-${Date.now()}.json`, 'application/json');
    });
    context.isLoading = false;
  },

  backupPartials(context) {
    const partials = [];
    // const templateList = response.body.data;
    const partialList = Object.keys(context.checkedRows).reduce((list, k) => list.concat(context.checkedRows[k]), []);
    if (!partialList.length) {
      context.$toast.open({
        message: 'Select partials first!',
        position: 'is-top',
        type: 'is-warning',
      });
      return;
    }
    context.isLoading = true;
    async.each(partialList, (elem, next) => {
      context.$http.get(`${Config.api}/partials/${elem._id}`).then((partial) => {
        partials.push(partial.body.data);
        next();
      }, (partial) => {
        Helpers.handleUnauthorized(partial, context);
        Helpers.handleInternal(partial, context);
        if (Helpers.isBad(partial)) {
          context.$toast.open({
            message: 'Bad request',
            position: 'is-top',
            type: 'is-warning',
          });
        }
      });
    }, () => {
      context.isLoading = false;
      Download(new Blob([JSON.stringify(partials)]), `partials-backup-${Date.now()}.json`, 'application/json');
    });
    context.isLoading = false;
  },

  restoreTemplates(file, context) {
    const reader = new FileReader();
    reader.onloadend = () => {
      const templates = JSON.parse(reader.result.toString('utf-8'));
      async.each(templates, (elem, next) => {
        elem.restore = true;
        context.$http.put(`${Config.api}/templates`, elem).then(() => {
          next();
        }, (response) => {
          Helpers.handleUnauthorized(response, context);
          Helpers.handleInternal(response, context);
          context.isLoading = false;
          if (Helpers.isBad(response)) {
            context.$toast.open({
              message: 'Bad request',
              position: 'is-top',
              type: 'is-warning',
            });
          }
        });
      }, () => {
        context.$router.go();
      });
    };
    reader.readAsText(file);
  },
  restorePartials(file, context) {
    const reader = new FileReader();
    reader.onloadend = () => {
      const partials = JSON.parse(reader.result);
      async.each(partials, (elem, next) => {
        elem.restore = true;
        context.$http.put(`${Config.api}/partials`, elem).then(() => {
          next();
        }, (response) => {
          Helpers.handleUnauthorized(response, context);
          Helpers.handleInternal(response, context);
          context.isLoading = false;
          if (Helpers.isBad(response)) {
            context.$toast.open({
              message: 'Bad request',
              position: 'is-top',
              type: 'is-warning',
            });
          }
        });
      }, () => {
        context.$router.go();
      });
    };
    reader.readAsText(file);
  },
};

export default Backup;
