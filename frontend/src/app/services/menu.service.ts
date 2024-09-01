import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Menu {
  id?: number;
  name: string;
  productIds: number[];
  productIdsString?: string;  
}

@Injectable({
  providedIn: 'root'
})
export class MenuService {
    private apiUrl = 'http://localhost:5230/api/MenuItems';
  
    constructor(private http: HttpClient) { }
  
    getMenus(): Observable<Menu[]> {
      return this.http.get<Menu[]>(this.apiUrl);
    }
  
    addMenu(menu: Menu): Observable<Menu> {
      return this.http.post<Menu>(this.apiUrl, menu);
    }
  
    updateMenu(id: number, menu: Menu): Observable<Menu> {
      return this.http.put<Menu>(`${this.apiUrl}/${id}`, menu);
    }
  
    deleteMenu(id: number): Observable<void> {
      return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
  }
  