import react from "@vitejs/plugin-react-swc";
import { defineConfig } from "vite";
import { cssColsGridLayout } from "./plugins/css-cols";

export default defineConfig({
	plugins: [
		react({
			tsDecorators: true,
			jsxImportSource: "@emotion/react",
		}),
		cssColsGridLayout,
	],
});
