<template>
  <div>
    <h4>Register</h4>
    <form>
      <label for="email" >E-Mail Address</label>
      <div>
        <input id="email" type="email" v-model="email" required autofocus>
      </div>
      <label for="password">Password</label>
      <div>
        <input id="password" type="password" v-model="password" required>
      </div>
      <label for="passwordConfirm">Confirm Password</label>
      <div>
        <input id="passwordConfirm" type="password" v-model="passwordConfirm" required>
      </div>
      <label for="firstname">Firstname</label>
      <div>
        <input id="firstname" type="text" v-model="firstname">
      </div>
      <label for="lastName">Lastname</label>
      <div>
        <input id="lastName" type="text" v-model="lastName">
      </div>
      <label for="birthday">Birthday</label>
      <div>
        <input id="birthday" type="date" v-model="birthday">
      </div>
      <div>
        <button type="submit" @click="handleSubmit">
          Register
        </button>
      </div>
    </form>
  </div>
</template>

<script>
    const API_URL = "https://budgetin-webapi-dev.azurewebsites.net/api";
  
    export default {
    props : ["nextUrl"],
    data(){
        return {
        name : "",
        email : "",
        password : "",
        passwordConfirm : "",
        is_admin : null
        }
    },
    methods : {
        handleSubmit(e) {
        e.preventDefault()
        if (this.password === this.passwordConfirm && this.password.length > 0)
        {
            let url = API_URL + "/Register"
            //if (this.is_admin != null || this.is_admin == 1) url = "http://localhost:3000/register-admin"
            this.$http.post(url, {
            email: this.email,
            password: this.password,
            passwordConfirm: this.passwordConfirm,
            firstName: this.firstName,
            lastName: this.lastName,
            birthday: this.birthday
            //is_admin: this.is_admin
            })
            .then(response => {
              localStorage.setItem('user', JSON.stringify(response.data.user));
              localStorage.setItem('jwt', response.data.token);
              if (localStorage.getItem('jwt') != null){
                this.$emit('loggedIn');
                if(this.$route.params.nextUrl != null){
                  this.$router.push(this.$route.params.nextUrl);
                }
                else{
                  this.$router.push('/');
                }
              }
            })
            .catch(error => {
              console.error(error);
            });
        } else {
            this.password = ""
            this.passwordConfirm = ""
            return alert("Passwords do not match")
        }
        }
    }
    }
</script>