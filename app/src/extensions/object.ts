declare global {
	interface Object {
		toJsonString(): string;
		toQueryString(): string;
	}

	interface ObjectConstructor {
		merge(receptor: object, donator: object): void;
		merge(receptor: object, donator: object, ignoreNull: boolean): void;
	}
}

Object.merge = function(receptor, donator, allowFalsy = false) {
  Object.keys(donator).forEach(function(field) {
    const value = donator[field];
	 const falsies = ["", null, undefined]
	 const isFalsy = falsies.some(x => x === value)

    if (allowFalsy || isFalsy == false) 
      receptor[field] = value;
  });
}

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
