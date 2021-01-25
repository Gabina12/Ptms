/* eslint no-param-reassign: ["error", { "props": false }] */
import async from 'async';
import Config from '@/config';
import Download from 'downloadjs';
import * as HttpCodes from 'http-status-codes';

const Helpers = {
  handleUnauthorized(response, context) {
    if (response.status === HttpCodes.UNAUTHORIZED) {
      context.$router.push({ name: 'login' });
    }
  },

  handleLargePayload(response, context) {
    if (response.status === HttpCodes.REQUEST_TOO_LONG) {
      context.$toast.open({
        duration: 5000,
        message: 'Request is too large, max size of requiest is 6mb',
        position: 'is-top',
        type: 'is-danger',
        queue: false,
      });
    }
  },

  handleInternal(response, context) {
    if ([HttpCodes.INTERNAL_SERVER_ERROR, HttpCodes.NOT_FOUND, 0].indexOf(response.status) >= 0) {
      const oneH = 1000 * 60 * 60;
      context.$toast.open({
        duration: oneH,
        message: 'Something happened to the server :(',
        position: 'is-top',
        type: 'is-danger',
        queue: false,
      });
      context.$snackbar.open({
        duration: oneH,
        message: 'Something happened, please try to reload page',
        type: 'is-danger',
        position: 'is-bottom-right',
        actionText: 'Reload',
        queue: false,
        onAction: () => context.$router.go(),
      });
    }
  },

  isBad(response) {
    return response.status === HttpCodes.BAD_REQUEST;
  },

  destroyTemplate(id, name, context, cb) {
    context.$dialog.confirm({
      title: 'Deleting template',
      message: `Are you sure you want to <b>delete</b> template: ${id}, ${name}`,
      confirmText: 'Delete',
      type: 'is-danger',
      hasIcon: true,
      onConfirm: () => {
        context.isLoading = true;
        context.$http.delete(`${Config.api}/templates/${id}`).then(() => {
          context.isLoading = false;
          context.$toast.open({
            message: 'Template deleted!',
            position: 'is-top',
            type: 'is-success',
          });
          if (cb) cb(id);
        }, (response) => {
          Helpers.handleUnauthorized(response, context);
          Helpers.handleInternal(response, context);
          if (Helpers.isBad(response)) {
            context.$toast.open({
              message: 'Cant delete template',
              position: 'is-top',
              type: 'is-warning',
            });
          }
          context.isLoading = false;
        });
      },
    });
  },
  destroyTemplateBulk(templates, context, cb) {
    if (!templates.length) {
      context.$toast.open({
        message: 'Select templates first!',
        position: 'is-top',
        type: 'is-warning',
      });
      return;
    }
    context.$dialog.confirm({
      title: 'Deleting templates',
      message: `Are you sure you want to <b>delete</b> ${templates.length} templates`,
      confirmText: 'Delete',
      type: 'is-danger',
      hasIcon: true,
      onConfirm: () => {
        context.isLoading = true;
        async.each(templates, (elem, next) => {
          context.$http.delete(`${Config.api}/templates/${elem._id}`).then(next, (template) => {
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
          context.$toast.open({
            message: 'Templates deleted!',
            position: 'is-top',
            type: 'is-success',
          });
          if (cb) cb(templates[0]);
        });
      },
    });
  },

  destroyPartial(id, name, context, cb) {
    context.$dialog.confirm({
      title: 'Deleting partial',
      message: `Are you sure you want to <b>delete</b> partial: ${id}, ${name}`,
      confirmText: 'Delete',
      type: 'is-danger',
      hasIcon: true,
      onConfirm: () => {
        context.isLoading = true;
        context.$http.delete(`${Config.api}/partials/${id}`).then(() => {
          context.isLoading = false;
          context.$toast.open({
            message: 'Partial deleted!',
            position: 'is-top',
            type: 'is-success',
          });
          if (cb) cb(id);
        }, (response) => {
          Helpers.handleUnauthorized(response, context);
          Helpers.handleInternal(response, context);
          if (Helpers.isBad(response)) {
            context.$toast.open({
              message: 'Cant delete partial',
              position: 'is-top',
              type: 'is-warning',
            });
          }
          context.isLoading = false;
        });
      },
    });
  },

  destroyPartialBulk(partials, context, cb) {
    if (!partials.length) {
      context.$toast.open({
        message: 'Select partials first!',
        position: 'is-top',
        type: 'is-warning',
      });
      return;
    }
    context.$dialog.confirm({
      title: 'Deleting partials',
      message: `Are you sure you want to <b>delete</b> ${partials.length} partials`,
      confirmText: 'Delete',
      type: 'is-danger',
      hasIcon: true,
      onConfirm: () => {
        context.isLoading = true;
        async.each(partials, (elem, next) => {
          context.$http.delete(`${Config.api}/partials/${elem._id}`).then(next, (partial) => {
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
          context.$toast.open({
            message: 'Partials deleted!',
            position: 'is-top',
            type: 'is-success',
          });
          if (cb) cb(partials[0]);
        });
      },
    });
  },

  previewTemplatePdf(id,version, context) {
    context.isLoading = true;
    context.$http.post(`${Config.api}/templates/${id}/render/pdf/${version}`, {
      caching: false,
    }, { responseType: 'arraybuffer' }).then((response) => {
      const fileType = 'application/pdf';
      const blob = new Blob([response.data], { type: fileType });
      Download(blob, `preview-${id}.pdf`, fileType);
      context.isLoading = false;
    }, (response) => {
      Helpers.handleUnauthorized(response, context);
      Helpers.handleInternal(response, context);
      if (Helpers.isBad(response)) {
        context.$toast.open({
          message: 'Cant preview template pdf',
          position: 'is-top',
          type: 'is-warning',
        });
        context.isLoading = false;
      }
    });
  },

  previewTemplateTxt(id,version, context) {
    context.$http.post(`${Config.api}/templates/${id}/render/txt/${version}`).then((response) => {
      context.$modal.open(`<iframe style="width: 100%; height: 100vh;" src="data:text/html;base64,${btoa(unescape(encodeURIComponent(response.bodyText)))}"></iframe>`);
    }, (response) => {
      Helpers.handleUnauthorized(response, context);
      Helpers.handleInternal(response, context);
      if (Helpers.isBad(response)) {
        context.$toast.open({
          message: 'Cant preview template html',
          position: 'is-top',
          type: 'is-warning',
        });
      }
    });
  },

  onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
  },

};

export default Helpers;
