import js from "@eslint/js"
import globals from "globals"
import tseslint from "typescript-eslint"
import pluginReact from "eslint-plugin-react"
import { defineConfig } from "eslint/config"

export default defineConfig([
  tseslint.configs.recommended,
  pluginReact.configs.flat.recommended,
  {
    plugins: { js },
    extends: ["js/recommended"],
    languageOptions: { globals: globals.browser },
    files: ["**/*.{js,mjs,cjs,ts,mts,cts,jsx,tsx}"]
  },
]);
