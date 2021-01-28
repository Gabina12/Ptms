<template>
  <div class="container-fluid">
    <NavBar></NavBar>
    <div class="container">
      <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"></b-loading>

      <button class="button field is-primary" @click="addNew">
        <b-icon icon="plus"></b-icon>
        <span>New User</span>
      </button>
      <b-table :data="users">
        <template slot-scope="props">
          <b-table-column width="300" label="ID" centered>
            <strong>{{ props.row._id }}</strong>
          </b-table-column>
          <b-table-column label="Name" centered>{{ props.row.user }}</b-table-column>
          <b-table-column label="Created" centered>{{ new Date(props.row.created).toString() }}</b-table-column>
          <b-table-column label="Creator" centered>{{ props.row.creator }}</b-table-column>
          <b-table-column label="Action" centered>
            <b-tooltip class="is-danger" label="Delete user">
              <button
                class="button is-small is-danger"
                @click="onDelete(props.row._id, props.row.user)"
              >
                <b-icon icon="delete-forever" size="is-small"></b-icon>
              </button>
            </b-tooltip>
          </b-table-column>
        </template>
        <template slot="empty">
          <section class="section">
            <div class="content has-text-grey has-text-centered">
              <p>
                <b-icon icon="emoticon-sad" size="is-large"></b-icon>
              </p>
              <p>No Users</p>
            </div>
          </section>
        </template>
      </b-table>
    </div>
  </div>
</template>

<style scoped>
button.is-small {
  margin: 2px;
}
</style>

<script>
// @ is an alias to /src
import Config from "@/config";
import NavBar from "@/components/NavBar.vue";
import NewUserForm from "@/components/NewUserForm.vue";
import Helpers from "@/helpers";

export default {
  name: "users",
  components: {
    NavBar
  },
  data() {
    return {
      isLoading: true,
      users: []
    };
  },
  mounted() {
    this.loadUsers();
  },
  methods: {
    addNew() {
      const that = this;
      this.$modal.open({
        parent: that,
        component: NewUserForm,
        hasModalCard: true,
        events: {
          added: () => that.loadUsers()
        }
      });
    },
    onDelete(id, user) {
      const that = this;
      that.$dialog.confirm({
        title: "Deleting user",
        message: `Are you sure you want to <b>delete</b> user: ${id}, ${user}`,
        confirmText: "Delete",
        type: "is-danger",
        hasIcon: true,
        onConfirm: () => {
          that.isLoading = true;
          that.$http.delete(`${Config.api}/users/${id}`).then(
            () => {
              that.isLoading = false;
              that.$toast.open({
                message: "Users deleted!",
                position: "is-top",
                type: "is-success"
              });
              that.loadUsers();
            },
            response => {
              Helpers.handleUnauthorized(response, that);
              Helpers.handleInternal(response, that);
              if (Helpers.isBad(response)) {
                that.$toast.open({
                  message: "Cant delete user",
                  position: "is-top",
                  type: "is-warning"
                });
              }
              that.isLoading = false;
            }
          );
        }
      });
    },
    loadUsers() {
      const that = this;
      that.$http.get(`${Config.api}/users`).then(
        response => {
          that.isLoading = false;
          that.users = response.body.data;
        },
        response => {
          Helpers.handleUnauthorized(response, that);
          Helpers.handleInternal(response, that);
          that.isLoading = false;
        }
      );
    }
  }
};
</script>
