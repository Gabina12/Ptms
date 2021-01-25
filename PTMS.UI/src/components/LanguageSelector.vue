<template>
  <div class="lang-selector">
    <b-select icon="earth">
      <option
            v-for="lang in languages"
            :value="lang.key"
            :key="lang.key">
      {{ lang.key.toUpperCase() }}
      </option>
    </b-select>
  </div>
</template>

<script>
export default {
  name: 'LanguageSelector',
  props: {
    onChange: Function,
    defaultLanguage: String,
    localization: Object,
  },
  computed: {
    languages() {
      const that = this;
      if (that.localization) { return Object.keys(that.localization).map(el => ({ key: el })); }
      return [];
    },
  },
};
</script>

<style scoped>

.lang-selector {
  padding: 20px;
}

</style>
