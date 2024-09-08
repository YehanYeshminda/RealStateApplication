import { Subscription } from "rxjs";

export function UnsubscribeHelper(...subscriptions: (Subscription | undefined)[]): void {
    subscriptions.forEach(sub => {
      if (sub) {
        sub.unsubscribe();
        console.log("Unsubscribed");
      }
    });
}