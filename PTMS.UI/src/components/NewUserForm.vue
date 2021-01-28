<template>
  <div class="card">
    <div class="card-content">
      <div style="margin: 15px;" class="is-size-2 has-text-centered">
        Add new user
      </div>
      <section>
        <b-field label="Username">
          <b-input v-model="username"
                   maxlength="25"
                   @keyup.enter.native="addNew"
                   size="is-medium">
          </b-input>
        </b-field>
        <b-field label="Password">
          <b-input type="password"
                   v-model="password"
                   maxlength="50"
                   size="is-medium"
                   @keyup.enter.native="addNew"
                   password-reveal>
          </b-input>
        </b-field>
        <b-field label="Role">
          <b-select v-model="role">
            <option
              v-for="rl in roles"
              :value="rl"
              :key="rl">
            {{ rl }}
            </option>
          </b-select>
        </b-field>
        <button @click="addNew" class="button is-success is-medium is-fullwidth">
          <b-icon icon="plus"></b-icon><span>Add</span>
        </button>
      </section>
    </div>
  </div>
</template>

<style>

.new-usr-form {
  margin-top: 50px;
  border-radius: 3px;
  padding: 25px;
  background-color: white;
}

</style>

<script>
import Config from '@/config';
import Helpers from '@/helpers';

export default {
  name: 'NewUserForm',
  props: {
    onAdd: Function,
  },
  data() {
    return {
      roles: ['admin', 'editor', 'viewer'],
      username: '',
      password: '',
      role: 'admin',
    };
  },
  methods: {
    addNew() {
      const that = this;
      if (that.checkLengths(that.username) && that.checkLengths(that.password)) {
        that.$http.put(`${Config.api}/users`, {
          user: that.username,
          password: that.password,
          role: that.role,
        }).then(() => {
          that.$parent.close();
          that.$emit('added');
        }, (response) => {
          Helpers.handleUnauthorized(response, that);
          Helpers.handleInternal(response, that);
          if (Helpers.isBad(response)) {
            this.$toast.open({
              message: 'Wront params, Check username/password lengths min:3 max:50',
              position: 'is-top',
              type: 'is-danger',
            });
          }
        });
      } else {
        this.$toast.open({
          message: 'Check username/password lengths min:3 max:50',
          position: 'is-top',
          type: 'is-danger',
        });
      }
    },
    checkLengths(str) {
      return str.length > 2 && str.length < 51;
    },
  },
};
</script>
