<template>
  <div class="app">
    <header>
      <h1>Currency Rates Dashboard</h1>
    </header>
    <main>
      <CurrencyRatesGrid />
    </main>
  </div>
</template>

<script setup lang="ts">
import CurrencyRatesGrid from './components/CurrencyRatesGrid.vue'
</script>

<style>
body {
  margin: 0;
  padding: 0;
  font-family: var(--font-family);
  background-color: #f5f5f5;
}

.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

header {
  background-color: var(--primary-color);
  color: white;
  padding: 1rem 2rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

header h1 {
  margin: 0;
  font-size: 1.5rem;
}

main {
  flex: 1;
  padding: 1rem;
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
  box-sizing: border-box;
}
</style>
