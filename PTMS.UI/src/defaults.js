const Defaults = {
  template: `<!DOCTYPE HTML>
<html>
  <head>
    <meta charset="utf-8">
    <title></title>
  </head>
  <body>
    {{Field}}
    {{#ObjectOrArray}}
      <h1>{{Attribute}}</h1>
    {{/ObjectOrArray}}
    {{>Partial}}
  </body>
</html>`,
  header: '<span style="font-size: 30px; width: 200px; height: 200px; background-color: black; color: white; margin: 20px;">Header</span>',
  footer: '<span style="font-size: 30px; width: 50px; height: 50px; background-color: red; color:black; margin: 20px;">Footer</span>',
};

export default Defaults;
