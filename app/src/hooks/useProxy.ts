import { useRef, useState } from "react";

const DELAY = 123;

export function useProxy<T>(data: T): T;
export function useProxy<T>(deep: boolean, data: T): T;
export function useProxy<T>(wait: number, deep: boolean, data: T): T;
export function useProxy<T>(...params: any[]) {
	params ||= [{}];

	const args = params.length;
	const data = params[params.length - 1];
	const deep = args > 1 ? params[1] : false;
	const wait = args < 3 ? DELAY : params[2];

	const [_, setTick] = useState(0);
	const stateRef = useRef<T>(data);
	const timeoutRef = useRef<any>(null);

	const createProxy = (target: any): any => {
		return new Proxy(target, {
			set(target, prop: string, value) {
				target[prop] = value;

				if (timeoutRef.current) clearTimeout(timeoutRef.current);
				const refresh = () => setTick((tick) => tick + 1);
				timeoutRef.current = setTimeout(refresh, wait);
				return true;
			},
			get(target, prop: string) {
				const value = target[prop];
				if (deep && value && typeof value === "object")
					return createProxy(value);
				else return value;
			},
		});
	};

	return createProxy(stateRef.current) as T;
}
