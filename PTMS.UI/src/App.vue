<template>
  <div class="full-screen" id="app">
    <router-view />
  </div>
</template>

<style>
html,
body,
.full-screen {
  margin: 0;
  padding: 0;
  height: 100%;
}

.has-white-bg {
  background: white;
}

.modal-content {
  background-color: white;
  width: 100%;
}

.collapse:not(.show) {
  display: block !important;
}

.navbar-item span {
  color: #fff;
}

.router-link-exact-active span {
  color: #fff !important;
}
.router-link-exact-active {
  background: #db0011;
}

a.navbar-item:hover,
a.navbar-item.is-active,
.navbar-link:hover,
.navbar-link.is-active {
  background-color: #db0011 !important;
  color: #ffffff !important;
}
</style>
