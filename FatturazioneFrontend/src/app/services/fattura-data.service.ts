import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FatturaDataService {
    private donumdoc: string = '';
    private year: number | null = null;
  
  constructor() {}

  getDonumdoc(): string {
    return this.donumdoc;
  }

  setDonumdoc(value: string): void {
    this.donumdoc = value;
  }

  getYear(): number | null  {
    return this.year
  }

  setYear(value: number | null): void {
    this.year = value;
  }
}
