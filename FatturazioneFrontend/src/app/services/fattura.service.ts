import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '/Users/matteo/source/repos/ClientApp/src/environment';

@Injectable({
  providedIn: 'root'
})
export class FatturaService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getFattura(donumdoc: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Fattura?donumdoc=${donumdoc}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  generaFileFatturazione(donumdoc: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/fatturazione/GeneraFileFatturazione`, { params: new HttpParams().set('donumdoc', donumdoc) })
    .pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred', error);
    return throwError(error);
  }
}
