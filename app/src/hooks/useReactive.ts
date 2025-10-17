import { useState, useRef } from 'react';

export function useReactive<T extends object>(initial: T, delay = 123) {
  const [, setTick] = useState(0);
  const stateRef = useRef(initial);
  const timeoutRef = useRef<any>(null);

  const proxy = new Proxy(stateRef.current, {
    set(target, prop: string, value) {
      target[prop as keyof T] = value

      if (timeoutRef.current) clearTimeout(timeoutRef.current)

      const refresh = () => setTick(tick => tick + 1)
      
      timeoutRef.current = setTimeout(refresh, delay)

      return true
    }
  })

  return proxy as T
}