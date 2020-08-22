<template>
  <div class="container">
    <h1 class="display-5">Login</h1>
    <form>
      <div class="form-group">
        <label for="username" >Username</label>
        <input id="username" type="text" class="form-control" v-model="username" required autofocus>
      </div>
      <div class="form-group">
        <label for="password" >Password</label>
        <input id="password" type="password" class="form-control" v-model="password" required>
      </div>
      <button type="submit" @click="handleSubmit">
        Login
      </button>
    </form>
    <div v-if="this.errors.length > 0" class="container alert alert-danger">
      <p v-for="error in errors" :key="error.errorMessage">error.errorMessage</p>
    </div>
  </div>
</template>

<script>
    export default {
        data(){
        return {
            username : "",
            password : "",
            errors: []
        }
        },
        methods : {
        handleSubmit(e){
            e.preventDefault()
            if (this.password.length > 0) {
            this.$http.post("/api/User/Login", {
                username: this.username,
                password: this.password
              },
              {
                headers: {
                  // 'application/json' is the modern content-type for JSON, but some
                  // older servers may use 'text/json'.
                  // See: http://bit.ly/text-json
                  'content-type': 'application/json'
                }
              }
            )
            .then(response => {
              this.errors = [];

              let is_admin = response.data.user.role == "Administrator" ? true : false;

              localStorage.setItem('user',JSON.stringify(response.data.user));
              localStorage.setItem('jwt',response.data.token);

              if (localStorage.getItem('jwt') != null){
                this.$emit('loggedIn');

                if(this.$route.params.nextUrl != null){
                  this.$router.push(this.$route.params.nextUrl);
                }
                else {
                  if(is_admin == 1){
                    this.$router.push('admin');
                  }
                  else {
                    this.$router.push('dashboard');
                  }
                }
              }
            })
            .catch(function (error) {
                console.error(error.response);
                this.errors = error.response.data.Error.errors;
            });
            }
        }
        }
    }
</script>