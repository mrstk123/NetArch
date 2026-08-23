import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../environments/environment';

export interface ProductSummary {
  id: number;
  name: string;
}

export interface CreateProductRequest {
  name: string;
}

@Injectable({ providedIn: 'root' })
export class ApiClient {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  getProducts() {
    return this.http.get<ProductSummary[]>(`${this.baseUrl}/products`);
  }

  createProduct(request: CreateProductRequest) {
    return this.http.post<{ id: number }>(`${this.baseUrl}/products`, request);
  }
}
