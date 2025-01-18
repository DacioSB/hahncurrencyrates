import { createApp } from 'vue'
import PrimeVue from 'primevue/config'
import App from './App.vue'

// PrimeVue components
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dropdown from 'primevue/dropdown'
import Button from 'primevue/button'
import Toast from 'primevue/toast'
import ToastService from 'primevue/toastservice'

// PrimeVue styles
import 'primevue/resources/themes/saga-blue/theme.css'
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'

// Utility styles
import './style.css'

const app = createApp(App)

app.use(PrimeVue, {
    ripple: true,
    inputStyle: 'filled'
})
app.use(ToastService)

// Register PrimeVue components
app.component('DataTable', DataTable)
app.component('Column', Column)
app.component('Dropdown', Dropdown)
app.component('Button', Button)
app.component('Toast', Toast)

app.mount('#app')