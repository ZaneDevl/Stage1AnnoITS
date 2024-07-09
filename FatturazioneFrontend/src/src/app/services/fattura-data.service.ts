import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FatturaDataService {
    private donumdoc: string = '';
  
  constructor() {}

  getDonumdoc(): string {
    return this.donumdoc;
  }

  setDonumdoc(value: string): void {
    this.donumdoc = value;
  }
}
