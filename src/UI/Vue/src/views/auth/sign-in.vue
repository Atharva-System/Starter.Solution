<template>
    <div
        class="flex justify-center items-center min-h-screen bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')] bg-cover bg-center">
        <div class="panel sm:w-[480px] m-6 max-w-lg w-full">
            <h2 class="font-bold text-2xl mb-3">Sign In</h2>
            <p class="mb-7">Enter your email and password to login</p>
            <form class="space-y-5" @submit.prevent="onSubmit()">
                <div>
                    <label for="email">Email</label>
                    <input id="email" type="email" class="form-input" placeholder="Enter Email" v-model="login.email" @input="v$.login.email.$touch()"/>
                    <template v-if="isSubmitted && v$.login.email.required.$invalid">
                        <p class="text-danger mt-1">Please fill the Email</p>
                    </template>
                    <template v-if="isSubmitted && v$.login.email.maxLength.$invalid">
                        <p class="text-danger mt-1">The email must be less than 100 characters.</p>
                    </template>
                </div>
                <div>
                    <label for="password">Password</label>
                    <input id="password" type="password" class="form-input" placeholder="Enter Password" v-model="login.password"  @input="v$.login.password.$touch()"/>
                    <template v-if="isSubmitted && v$.login.password.required.$invalid">
                        <p class="text-danger mt-1">Please fill the Password</p>
                    </template>
                    <template v-if="isSubmitted && v$.login.password.minLength.$invalid">
                        <p class="text-danger mt-1">The password must be longer than 6 characters.</p>
                    </template>

                </div>
                <div>
                    <label class="cursor-pointer">
                        <p class="text-left">
                            <router-link to="/auth/forgot-password" class="text-primary font-bold hover:underline">Forgot
                                Password</router-link>
                        </p>
                    </label>
                </div>
                <button type="submit" class="btn btn-primary w-full">SIGN IN</button>
            </form>

        </div>
    </div>
</template>
<script lang="ts">
import { useMeta } from '@/composables/use-meta';
import { defineComponent } from 'vue';
import useVuelidate from '@vuelidate/core';
import type { ILogin } from "@/types";
import { maxLength, minLength, required } from '@vuelidate/validators';
import authService from '@/services/auth.service';

useMeta({ title: 'Login' });

export default defineComponent({
    setup() {
        return { v$: useVuelidate() };
    },
    data() {
        return {
            login: { email: "", password: "" } as ILogin,
            postError: false,
            postErrorMessage: "",
            isSubmitted: false
        }
    },
    computed: {

    },
    validations: {
        login: {
            email: {
                required,
                maxLength: maxLength(100)
            },
            password: {
                required,
                minLength: minLength(6),
            }
        }
    },
    methods: {
        onSubmit() {
            this.isSubmitted = true;

            if (this.v$.login.$invalid) {
                return;
            }

            const promise = authService.login(this.login);

            promise.then(rs => {
                this.$router.push("/");
            });

            promise.catch(err => {
                console.log(err.response.data.Messages[0])
            })
        }
    },
    created() {

    }
});

</script>
