module.exports = {
    content: [
        "./src/**/*.{js,jsx,ts,tsx}",
    ],
    theme: {
        extend: {}
    },
    plugins: [
        require('flowbite/plugin'),
        require('@tailwindcss/typography')
    ],
    content: [
        // ...
        'node_modules/flowbite-react/**/*.{js,jsx,ts,tsx}'
    ]
}