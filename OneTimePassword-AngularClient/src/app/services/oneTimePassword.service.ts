import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class OneTimePasswordService {
  private baseURL: string = "https://localhost:7000/";

  constructor(private http: HttpClient) {

  }

  public generateOneTimePasswordForUser(userId: string) {
    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(userId);
    return this.http.post(this.baseURL + 'OneTimePassword/Generate', body, { 'headers': headers }).toPromise();
  }

  public getValidOneTimePassword(userId: string) {
    return this.http.get<OneTimePassword>(this.baseURL + 'OneTimePassword/GetValidPassword?userId=' + userId).toPromise();
  }
}

interface OneTimePassword {
  value: string;
  expirationInSeconds: number;
}


