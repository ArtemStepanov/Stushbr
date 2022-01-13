import React, { Component } from 'react';

export class FetchItems extends Component {
  static displayName = FetchItems.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
        <tr>
          <th>Id</th>
          <th>Display Name</th>
          <th>Description</th>
          <th>Price</th>
          <th>Type</th>
          <th>Is Available</th>
          <th>Available Since</th>
          <th>Available Before</th>
        </tr>
        </thead>
        <tbody>
        {forecasts.map(forecast =>
          <tr key={forecast.id}>
            <td>{forecast.id}</td>
            <td>{forecast.displayName}</td>
            <td>{forecast.description}</td>
            <td>{forecast.price}</td>
            <td>{forecast.type}</td>
            <td>{forecast.isAvailable ? 'yes' : 'no'}</td>
            <td>{forecast.availableSince}</td>
            <td>{forecast.availableBefore}</td>
          </tr>
        )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchItems.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('items/available');
    const data = await response.json();
    const resp2 = await fetch('items/c3f2c680edc345168643179c2d568227/order', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        clientInfo: {
          firstName: "Artem",
          secondName: "Stepanov",
          email: "test@test.test",
          phoneNumber: "89998030386"
        }
      })
    });
    const data2 = await resp2.json();
    console.log(data2);
    this.setState({ forecasts: data, loading: false });
  }
}
