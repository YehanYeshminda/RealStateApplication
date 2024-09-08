import { FormGroup } from "@angular/forms";

export function getDateOffset(date: string, offset: number): string {
  const currentDate = new Date(date);
  currentDate.setDate(currentDate.getDate() + offset);
  const year = currentDate.getFullYear();
  const month = ('0' + (currentDate.getMonth() + 1)).slice(-2);
  const day = ('0' + currentDate.getDate()).slice(-2);
  return `${year}-${month}-${day}`;
}

export function getCurrentDate(): string {
  const currentDate = new Date();
  const year = currentDate.getFullYear();
  const month = ('0' + (currentDate.getMonth() + 1)).slice(-2);
  const day = ('0' + currentDate.getDate()).slice(-2);
  return `${year}-${month}-${day}`;
}

