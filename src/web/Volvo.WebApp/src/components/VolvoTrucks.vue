<template>
  <div class="container-fluid mt-4">
    <h1 class="h1">Trucks</h1>
    <b-alert :show="loading" variant="info">Loading...</b-alert>
    <b-row>
      <b-col>
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Model</th>
              <th>Model Year</th>
              <th>Manufacture Year</th>
              <th>VIN(Vehicle Identification Number)</th>
              <th>&nbsp;</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="record in records" :key="record.id">
              <td>{{ record.model.name }}</td>
              <td>{{ record.model.modelYear }}</td>
              <td>{{ record.manufactureYear }}</td>
              <td>{{ record.vin.number }}</td>
              <td class="text-right">
                <a href="#" @click.prevent="updateTruckRecord(record)">Edit</a>
                -
                <a href="#" @click.prevent="deleteTruckRecord(record.id)"
                  >Delete</a
                >
              </td>
            </tr>
          </tbody>
        </table>
      </b-col>
      <b-col lg="3">
        <b-card
          :title="model.id ? 'Edit Truck ID#' + model.id : 'New Truck Record'"
        >
          <form @submit.prevent="createTruckRecord">
            <b-alert
              :show="dismissCountDown"
              dismissible
              @variant="dismissClass"
              @dismissed="dismissCountDown = 0"
              @dismiss-count-down="countDownChanged"
            >
              {{ dismissMessage }}
            </b-alert>
            <b-form-select v-model="model.modelId" :options="options" />
            <b-form-group label="Manufacture Year">
              <b-form-input
                rows="4"
                v-model.number="model.manufactureYear"
                type="number"
              ></b-form-input>
            </b-form-group>
            <b-form-group label="VIN(Vehicle Identification Number)">
              <b-form-input
                rows="4"
                v-model="model.vin"
                type="text"
                maxlength="17"
              ></b-form-input>
            </b-form-group>
            <div>
              <b-btn type="submit" variant="success">Save Record</b-btn>
            </div>
          </form>
        </b-card>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import api from "@/VolvoApiService";

export default {
  data() {
    return {
      loading: false,
      records: [],
      options: [],
      dismissMessage: "",
      dismissSecs: 5,
      dismissCountDown: 0,
      dismissClass: "danger",
      showDismissibleAlert: false,
      model: {
        modelId: "",
      },
    };
  },
  async created() {
    this.getAllTruks();
    this.getOptions();
  },
  methods: {
    async getAllTruks() {
      this.loading = true;
      try {
        this.records = await api.getAllTruck(0);
        console.log(this.records);
      } finally {
        this.loading = false;
      }
    },
    async getOptions() {
      this.loading = true;
      try {
        let modelsTruck = await api.getAllModel();
        let defaultValue = { value: "", text: "Please select an Model" };
        this.options = modelsTruck.map((v) => ({
          value: v.id,
          text: `${v.name} - ${v.modelYear}`,
        }));
        this.options.unshift(defaultValue);
      } finally {
        this.loading = false;
      }
    },
    async updateTruckRecord(truckRecord) {
      this.model = Object.assign(
        {},
        {
          id: truckRecord.id,
          manufactureYear: truckRecord.manufactureYear,
          modelId: truckRecord.modelId,
          vin: truckRecord.vin.number,
        }
      );
    },
    async createTruckRecord() {
      const isUpdate = !!this.model.id;

      if (!this.model.modelId) return this.showAlert(true, "Model is requerid");

      if (!this.model.manufactureYear || this.model.manufactureYear < 1927)
        return this.showAlert(
          true,
          "Manufacture Year Must be greater than 1927"
        );

      if (!this.model.vin || this.model.vin.length < 17)
        return this.showAlert(true, "Vin Must be greater than 17");

      let result = [];
      if (isUpdate) {
        result = await api.update(this.model);
      } else {
        result = await api.create(this.model);
      }

      if (!result.success) {
        result.errors
          ? this.showAlert(result.errors.Mensagens.join(". "))
          : this.showAlert("Error connect in server");
        return;
      } else {
        this.showAlert(false, "Salvo com Sucesso");
      }

      this.model = { modelId: "" };

      await this.getAllTruks();
    },
    async deleteTruckRecord(id) {
      if (confirm("Are you sure you want to delete this record?")) {
        if (this.model.id === id) {
          this.model = {};
        }

        let result = await api.delete(id);
        if (!result.success) {
          result.errors
            ? this.showAlert(result.errors.Mensagens.join(". "))
            : this.showAlert("Error connect in server");
          return;
        } else {
          this.showAlert(false, "Removido com Sucesso");
        }

        await this.getAllTruks();
      }
    },
    countDownChanged(dismissCountDown) {
      this.dismissCountDown = dismissCountDown;
    },
    showAlert(error, messsage) {
      this.dismissCountDown = this.dismissSecs;
      this.dismissMessage = messsage;
      this.dismissClass = error ? "danger" : "success";
    },
  },
};
</script>
