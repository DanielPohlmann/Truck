// Vue imports
import Vue from 'vue'
import Router from 'vue-router'

// our own imports
import Hello from '@/components/Hello'
import VolvoTrucks from '@/components/VolvoTrucks'

/*
Vue.use(Auth, {
  issuer: '',
  client_id: '',
  redirect_uri: '',
  scope: ''
})
*/


Vue.use(Router)

let router = new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'Hello',
      component: Hello
    },
    /*{
      path: '/implicit/callback',
      component: Auth.handleCallback()
    },*/
    {
      path: '/volvo-trucks',
      name: 'VolvoTrucks',
      component: VolvoTrucks,
      meta: {
        requiresAuth: true
      }
    },
  ]
})

/*router.beforeEach(Vue.prototype.$auth.authRedirectGuard())*/

export default router
