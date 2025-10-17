declare global {
   interface String {
      capitalize(): string
   }
}

String.prototype.capitalize = function() {
   if (!this) return this
   else return this[0].toUpperCase() + this.slice(1).toLowerCase();
}