<template>
  <div>
    <h4>Login</h4>
    <form>
      <label for="username" >Username</label>
      <div>
        <input id="username" type="text" v-model="username" required autofocus>
      </div>
      <div>
        <label for="password" >Password</label>
        <div>
          <input id="password" type="password" v-model="password" required>
        </div>
      </div>
      <div>
        <button type="submit" @click="handleSubmit">
          Login
        </button>
      </div>
    </form>
  </div>
</template>

<script>
    const API_URL = "https://budgetin-webapi-dev.azurewebsites.net/api";

    export default {
        data(){
        return {
            username : "",
            password : ""
        }
        },
        methods : {
        handleSubmit(e){
            e.preventDefault()
            if (this.password.length > 0) {
            this.$http.post(API_URL + "/User/Login", {
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
                let is_admin = response.data.user.role == "Administrator" ? true : false;
                localStorage.setItem('user',JSON.stringify(response.data.user))
                localStorage.setItem('jwt',response.data.token)
                if (localStorage.getItem('jwt') != null){
                    this.$emit('loggedIn')
                    if(this.$route.params.nextUrl != null){
                    this.$router.push(this.$route.params.nextUrl)
                    }
                    else {
                    if(is_admin== 1){
                        this.$router.push('admin')
                    }
                    else {
                        this.$router.push('dashboard')
                    }
                    }
                }
            })
            .catch(function (error) {
                console.error(error.response);
            });
            }
        }
        }
    }
</script>