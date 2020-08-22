import Vue from 'vue';
import Vuex from 'vuex';
import axios from 'axios';

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    authStatus: '',
    token: localStorage.getItem('token') || '',
    user: {}
  },
  mutations: {
    auth_request(state){
      state.authStatus = 'loading';
    },
    auth_success(state, payload){
      state.authStatus = 'success';
      state.token = payload.token;
      state.user = payload.user;
    },
    auth_error(state){
      state.authStatus = 'error';
    },
    logout(state){
      state.authStatus = '';
      state.token = '';
    }
  },
  actions: {
    login({commit}, loginData){
      return new Promise((resolve, reject)=> {
        commit('auth_request');
        axios({url: '/api/User/Login', data: loginData, method: 'POST' })
        .then(resp => {
          const user = JSON.stringify(resp.data.user);
          const token = resp.data.token;
          localStorage.setItem('token', token);
          localStorage.setItem('user', user);
          axios.defaults.headers.common['Authorization'] = 'Bearer ' + token; // Добавил Bearer
          commit('auth_success', {
            token: token,
            user: user
          });
          resolve(resp);
        })
        .catch(err => {
          commit('auth_error');
          localStorage.removeItem('token');
          reject(err);
        })
      })
    },
    register({commit}, registerData){
      return new Promise((resolve, reject) => {
        commit('auth_request')
        axios({url: '/api/User/Register', data: registerData, method: 'POST' })
        .then(resp => {
          // const token = resp.data.token;
          // const user = resp.data.user;
          // localStorage.setItem('token', token);
          // axios.defaults.headers.common['Authorization'] = 'Bearer ' + token; // Добавил Bearer
          // commit('auth_success', token, user)
          resolve(resp);
        })
        .catch(err => {
          // commit('auth_error', err);
          // localStorage.removeItem('token');
          reject(err);
        })
      })
    },
    logout({commit}){
      return new Promise((resolve) => {
        commit('logout');
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization'];
        resolve();
      })
    }
  },
  modules: {
  },
  getters: {
    isLoggedIn: state => !!state.token,
    authStatus: state => state.authStatus,
    user: state => state.user,
    role: state => state.user.role
  }
})
