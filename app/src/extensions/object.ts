declare global {
   interface Object {
      toString(type?: 'JSON'|'query'):string
   }
}

Object.toString = function(type?: string) {
   if (!type) return this.toString()
   if (type == 'JSON') return JSON.stringify(this)
   if (type == 'query') return '?' + new URLSearchParams(this as any).toString()
   return this?.toString()
}

      
      