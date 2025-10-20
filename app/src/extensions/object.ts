declare global {
	interface Object {
		toJsonString(): string;
		toQueryString(): string;
	}

	interface ObjectConstructor {
		merge(from: object, to: object): object;
		merge(from: object, to: object, ignoreNull: boolean): object;
	}
}

Object.merge = (from, to, ignore = false) => {
	if (!from) return to;
	if (!to) return from;

	return Object.entries(from)
		.filter(([_, val]) => !ignore || val !== null)
		.filter(([_, val]) => !ignore || val !== undefined)
		.reduce((obj, [key, val]) => (obj[key] = val ? obj : obj), {});
};

// OBSERVACAO: nunca use o Object.prototype = function !!!
// usar o defineProperty ao invés do Object.prototype mais comum
// evita problemas de spread operator com objetos que quebram o app

Object.defineProperty(Object.prototype, "toJsonString", {
	value: function () {
		JSON.stringify(this);
	},
	enumerable: false,
});

Object.defineProperty(Object.prototype, "toQueryString", {
	enumerable: false,
	value: function () {
		return toQueryString(this);
	},
});

function toQueryString(that) {
	if (!that) return "";

	const initial = {} as Record<string, string>;

	const isNullOrEndefined = (_, value) => value !== undefined && value !== null;

	const convertValueToString = (obj, [key, val]) => {
		if (typeof val !== "object") obj[key] = String(val);

		Object.entries(val).forEach(([k, v]) => {
			obj[`${key}.${k}`] = v;
		});

		return obj;
	};

	const instance = Object.entries(that).filter(isNullOrEndefined).reduce(convertValueToString, initial);

	return `?${new URLSearchParams(instance).toString()}`;
}
