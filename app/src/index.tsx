/// <reference path="./index.d.ts" />

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { LoadingProvider } from "./components/loading";
import { ToastProvider } from "./components/toast";
import "./index.d.ts";
import "./extensions";

const queryClient = new QueryClient();
const rootHTML = document.getElementById("root");

ReactDOM.createRoot(rootHTML!).render(
	<React.StrictMode>
		<BrowserRouter>
			<QueryClientProvider client={queryClient}>
				<LoadingProvider>
					<ToastProvider>
						<App />
					</ToastProvider>
				</LoadingProvider>
			</QueryClientProvider>
		</BrowserRouter>
	</React.StrictMode>,
);
