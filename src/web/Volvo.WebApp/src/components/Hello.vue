<template>
  <div class="hello">
    <h1>{{ title }}</h1>
    <h2>
      Almost 100,000 employees. 12 brands. 190 markets. Production facilities in
      18 countries. Trucks, buses, construction equipment and marine and
      industrial engines. Complete solutions for financing and service. This is
      who we are. #WeAreVolvoGroup
    </h2>
  </div>
</template>

<script>
export default {
  data() {
    return {
      title: "Welcome to Volvo Group",
    };
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #35495e;
}
</style>
