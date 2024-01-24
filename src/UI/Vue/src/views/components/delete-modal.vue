<template>
    <Dialog as="div" class="relative z-50">
        <TransitionChild as="template" enter="duration-300 ease-out" enter-from="opacity-0" enter-to="opacity-100"
            leave="duration-200 ease-in" leave-from="opacity-100" leave-to="opacity-0">
            <DialogOverlay class="fixed inset-0 bg-[black]/60" />
        </TransitionChild>

        <div class="fixed inset-0 overflow-y-auto">
            <div class="flex min-h-full items-center justify-center px-4 py-8">
                <TransitionChild as="template" enter="duration-300 ease-out" enter-from="opacity-0 scale-95"
                    enter-to="opacity-100 scale-100" leave="duration-200 ease-in" leave-from="opacity-100 scale-100"
                    leave-to="opacity-0 scale-95">
                    <DialogPanel
                        class="panel border-0 p-0 rounded-lg overflow-hidden w-full max-w-lg text-black dark:text-white-dark">
                        <button type="button"
                            class="absolute top-4 ltr:right-4 rtl:left-4 text-gray-400 hover:text-gray-800 dark:hover:text-gray-600 outline-none"
                            @click="onClose">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" viewBox="0 0 24 24"
                                fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round"
                                stroke-linejoin="round" class="w-6 h-6">
                                <line x1="18" y1="6" x2="6" y2="18"></line>
                                <line x1="6" y1="6" x2="18" y2="18"></line>
                            </svg>
                        </button>
                        <div
                            class="text-lg font-medium bg-[#fbfbfb] dark:bg-[#121c2c] ltr:pl-5 rtl:pr-5 py-3 ltr:pr-[50px] rtl:pl-[50px]">
                            {{ title }}
                        </div>
                        <div class="p-5 text-center">
                            <div class="text-white bg-danger ring-4 ring-danger/30 p-4 rounded-full w-fit mx-auto">
                                <svg width="24" height="24" viewBox="0 0 24 24" fill="none"
                                    xmlns="http://www.w3.org/2000/svg" class="w-5 h-5">
                                    <path opacity="0.5"
                                        d="M9.17065 4C9.58249 2.83481 10.6937 2 11.9999 2C13.3062 2 14.4174 2.83481 14.8292 4"
                                        stroke="currentColor" stroke-width="1.5" stroke-linecap="round">
                                    </path>
                                    <path d="M20.5001 6H3.5" stroke="currentColor" stroke-width="1.5"
                                        stroke-linecap="round"></path>
                                    <path
                                        d="M18.8334 8.5L18.3735 15.3991C18.1965 18.054 18.108 19.3815 17.243 20.1907C16.378 21 15.0476 21 12.3868 21H11.6134C8.9526 21 7.6222 21 6.75719 20.1907C5.89218 19.3815 5.80368 18.054 5.62669 15.3991L5.16675 8.5"
                                        stroke="currentColor" stroke-width="1.5" stroke-linecap="round">
                                    </path>
                                    <path opacity="0.5" d="M9.5 11L10 16" stroke="currentColor" stroke-width="1.5"
                                        stroke-linecap="round"></path>
                                    <path opacity="0.5" d="M14.5 11L14 16" stroke="currentColor" stroke-width="1.5"
                                        stroke-linecap="round"></path>
                                </svg>
                            </div>
                            <div class="text-base sm:w-3/4 mx-auto mt-5">{{ message }}</div>

                            <div class="flex justify-center items-center mt-8">
                                <button type="button" class="btn btn-outline-danger" @click="onClose">Cancel</button>
                                <button type="button" class="btn btn-primary ltr:ml-4 rtl:mr-4"
                                    @click="onDelete">Delete</button>
                            </div>
                        </div>
                    </DialogPanel>
                </TransitionChild>
            </div>
        </div>
    </Dialog>
</template>
  
<script lang="ts">
import { TransitionChild, Dialog, DialogPanel, DialogOverlay } from '@headlessui/vue';
import { PropType } from 'vue';

export default {
    components: {
        TransitionChild, Dialog, DialogPanel, DialogOverlay
    },
    props: {
        message: String,
        title: String,
        onDelete: {
            type: Function as PropType<(event: MouseEvent) => void>,
            required: true,
        },
        onClose: {
            type: Function as PropType<(event: MouseEvent) => void>,
            required: true,
        },
    },
};
</script>
  