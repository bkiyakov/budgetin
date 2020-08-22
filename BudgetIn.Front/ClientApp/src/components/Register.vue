<template>
  <div v-if="!registred" class="container">
    <h1 class="display-5">Register</h1>
    <form>
      <div class="form-group">
        <label for="email" >E-Mail Address</label>
        <input id="email" type="email" class="form-control" v-model="email" required autofocus>
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <input id="password" type="password" class="form-control" v-model="password" required>
      </div>
      <div class="form-group">
        <label for="passwordConfirm">Confirm Password</label>
        <input id="passwordConfirm" type="password" class="form-control" v-model="passwordConfirm" required>
      </div>
      <div class="form-group">
        <label for="firstname">Firstname</label>
        <input id="firstname" type="text" class="form-control" v-model="firstname">
      </div>
      <div class="form-group">
        <label for="lastName">Lastname</label>
        <input id="lastName" type="text" class="form-control" v-model="lastName">
      </div>
      <div class="form-group">
        <label for="birthday">Birthday</label>
        <input id="birthday" type="date" class="form-control" v-model="birthday">
      </div>
      <button type="submit" class="btn btn-primary" style="margin-top: 20px" @click="handleSubmit">
        Register
      </button>
    </form>
    <div v-if="responseFailed" class="alert alert-danger" role="alert" style="margin-top: 15px">
      <h5>Регистрация не удалась</h5>
      <p v-for="error in this.errors" :key="error.errorMessage">{{error.errorMessage}}</p>
    </div>
  </div>
  <div v-else class="container">
    <h1 class="display-5">
      Вы успешно зарегистрировались!
    </h1>
    <h3>
      Перейдите на страницу авторизации для входа на сайт.
    </h3>
    <a href="/login" class="btn btn-outline-success">Login</a>
  </div>
</template>

<script>
    export default {
    props : ["nextUrl"],
    data(){
        return {
          name : "",
          email : "",
          password : "",
          passwordConfirm : "",
          is_admin : null,
          registred : false,
          responseFailed: false,
          errors: []
        }
    },
    methods : {
        handleSubmit(e) {
        e.preventDefault()
        if (this.password === this.passwordConfirm && this.password.length > 0)
        {
            let url = "/api/User/Register"
            
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
              //localStorage.setItem('user', JSON.stringify(response.data.user));
              //localStorage.setItem('jwt', response.data.token);
              // if (localStorage.getItem('jwt') != null){
              //   this.$emit('loggedIn');
              //   if(this.$route.params.nextUrl != null){
              //     this.$router.push(this.$route.params.nextUrl);
              //   }
              //   else{
              //     this.$router.push('/');
              //   }
              // }
              this.registred = true;
              this.responseFailed = false;
            })
            .catch(error => {
              this.registred = false;
              this.responseFailed = true;
              this.errors = error.response.data.Error.errors; // TODO Поменять на конкретное сообщение об ошибке
              console.log(error.response.data.Error.errors);
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