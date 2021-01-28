import Defaults from '@/defaults';

function newTemplate() {
  return {
    _id: null,
    name: '',
    description: '',
    caching: false,
    loadExternalSources: false,
    template: Defaults.template,
    created: Date.now(),
    creator: '',
    updated: Date.now(),
    editor: '',
    category: '',
    options: {
      scale: 1,
      displayHeaderFooter: false,
      headerTemplate: Defaults.header,
      footerTemplate: Defaults.footer,
      printBackground: false,
      landscape: false,
      format: 'Letter',
      margin: {
        top: '0px',
        bottom: '0px',
        right: '0px',
        left: '0px',
      },
    },
  };
}

export default newTemplate;
