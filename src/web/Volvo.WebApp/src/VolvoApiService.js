import Vue from 'vue'
import axios from 'axios'

const client = axios.create({
  baseURL: 'http://localhost:44360',
  json: true
})

export default {
  async execute(method, resource, data) {
    //const accessToken = await Vue.prototype.$auth.getAccessToken()
    return client({
      method,
      url: resource,
      data,
      /*headers: {
        Authorization: `Bearer ${accessToken}`
      }*/
    }).then(req => {
      return req.data
    }).catch(function (error) {
      return error.response.data;
      //return Promise.reject(error.response.data)
    })
  },
  getAllModel() {
    return this.execute('GET', '/model/getAll')
  },
  getAllTruck(page = 0) {
    return this.execute('GET', '/truck/getAll', `?page=${page}`)
  },
  create(data) {
    return this.execute('POST', '/truck/create', data)
  },
  update(data) {
    return this.execute('PUT', '/truck/edit', data)
  },
  delete(id) {
    return this.execute('DELETE', `/truck/delete`, { id: id })
  }
}
