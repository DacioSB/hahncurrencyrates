<template>
  <div class="card">
    <Toast position="top-right" />
    <DataTable
      :value="currencyRates"
      :loading="loading"
      :paginator="true"
      :rows="10"
      :sortField="filter.sortBy"
      :sortOrder="filter.sortDescending ? -1 : 1"
      @sort="onSort"
      stripedRows
      showGridlines
      tableStyle="min-width: 50rem"
    >
      <template #header>
        <div class="flex justify-content-between align-items-center">
          <h2>Currency Rates</h2>
          <div class="flex gap-2">
            <Button 
              icon="pi pi-refresh" 
              @click="loadData" 
              :loading="loading"
              severity="secondary"
              text
            />
            <Button
              icon="pi pi-filter-slash"
              @click="clearFilters"
              :disabled="loading"
              severity="secondary"
              text
            />
            <Dropdown
              v-model="filter.baseCurrency"
              :options="availableCurrencies"
              placeholder="Base Currency"
              class="w-full md:w-14rem"
              @change="loadData"
            />
            <MultiSelect
              v-model="filter.targetCurrencies"
              :options="availableCurrencies"
              placeholder="Target Currencies"
              class="w-full md:w-14rem"
              @change="loadData"
              display="chip"
            />
          </div>
        </div>
      </template>

      <Column field="baseCurrency" header="Base Currency" sortable></Column>
      <Column field="targetCurrency" header="Target Currency" sortable></Column>
      <Column field="rate" header="Rate" sortable>
        <template #body="{ data }">
          {{ data.rate.toFixed(6) }}
        </template>
      </Column>
      <Column field="lastUpdated" header="Last Updated" sortable>
        <template #body="{ data }">
          {{ new Date(data.lastUpdated).toLocaleString() }}
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Dropdown from 'primevue/dropdown';
import MultiSelect from 'primevue/multiselect';
import Button from 'primevue/button';
import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';
import type { CurrencyRate, CurrencyRatesFilter } from '../types/CurrencyRate';
import { currencyRatesService } from '../services/currencyRatesService';

const toast = useToast();
const availableCurrencies = ['USD', 'EUR', 'GBP', 'JPY'];
const currencyRates = ref<CurrencyRate[]>([]);
const loading = ref(false);
const filter = ref<CurrencyRatesFilter>({
  baseCurrency: undefined,
  targetCurrencies: [],
  sortBy: undefined,
  sortDescending: false,
});

const clearFilters = () => {
  filter.value.baseCurrency = undefined;
  filter.value.targetCurrencies = [];
  filter.value.sortBy = undefined;
  filter.value.sortDescending = false;
  loadData();
};

const loadData = async () => {
  try {
    loading.value = true;
    currencyRates.value = await currencyRatesService.getCurrencyRates(filter.value);
  } catch (error) {
    console.error('Failed to load currency rates:', error);
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: 'Failed to load currency rates. Please try again.',
      life: 5000
    });
  } finally {
    loading.value = false;
  }
};

const onSort = (event: any) => {
  filter.value.sortBy = event.sortField;
  filter.value.sortDescending = event.sortOrder === -1;
  loadData();
};

onMounted(() => {
  loadData();
});
</script>

<style scoped>
.card {
  padding: 2rem;
  margin: 1rem;
  background: white;
  border-radius: 10px;
  box-shadow: 0 2px 1px -1px rgba(0, 0, 0, 0.2),
              0 1px 1px 0 rgba(0, 0, 0, 0.14),
              0 1px 3px 0 rgba(0, 0, 0, 0.12);
}
</style>
