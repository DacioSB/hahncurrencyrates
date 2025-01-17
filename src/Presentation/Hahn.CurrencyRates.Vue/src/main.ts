import { createApp } from 'vue'
import PrimeVue from 'primevue/config'
import App from './App.vue'

// PrimeVue styles
import 'primevue/resources/themes/lara-light-blue/theme.css'
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'

const app = createApp(App)

app.use(PrimeVue, {
    ripple: true,
    inputStyle: 'filled'
})

app.mount('#app')
