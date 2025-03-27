async function fetchMarketData() {
    try {
        let startDate = document.getElementById("startDate").value;
        let endDate = document.getElementById("endDate").value;
        let url = `${BASE_URL}/MarketData/GetChartData`;

        if (startDate) url += `?startDate=${startDate}`;
        if (endDate) url += startDate ? `&endDate=${endDate}` : `?endDate=${endDate}`;

        let response = await fetch(url);
        if (!response.ok) throw new Error("Failed to fetch data");

        let dataResponse = await response.json();
        let labels = dataResponse.data.map(d => new Date(d.date).toLocaleString());
        let data = dataResponse.data.map(d => d.price);

        // Cập nhật thống kê
        document.getElementById("minPrice").textContent = dataResponse.dataSummary.min;
        document.getElementById("maxPrice").textContent = dataResponse.dataSummary.max;
        document.getElementById("avgPrice").textContent = dataResponse.dataSummary.average;
        document.getElementById("mostExpensiveHour").textContent = dataResponse.dataSummary.mostExpensiveHour;
        document.getElementById("bestBuyTime").textContent = dataResponse.dataSummary.bestBuyTime;
        document.getElementById("bestSellTime").textContent = dataResponse.dataSummary.bestSellTime;

        // Kiểm tra nếu marketChart đã tồn tại và là một đối tượng Chart hợp lệ thì hủy nó trước khi tạo mới
        if (marketChart instanceof Chart) {
            marketChart.destroy();
        }

        // Vẽ lại biểu đồ
        var ctx = document.getElementById('marketChart').getContext('2d');
        marketChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Market Price',
                    data: data,
                    borderColor: 'blue',
                    borderWidth: 2,
                    pointRadius: 3,
                    fill: false
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: 'top' },
                    title: { display: true, text: 'Price summary chart' },
                    zoom: {
                        pan: { enabled: true, mode: 'xy' },
                        zoom: {
                            wheel: { enabled: true },
                            pinch: { enabled: true },
                            mode: 'x',
                        }
                    },
                    tooltip: {
                        callbacks: {
                            title: (tooltipItems) => tooltipItems[0].label,
                            label: (tooltipItem) => `📊 Price: ${tooltipItem.raw}`
                        }
                    }
                },
                scales: {
                    x: {
                        type: 'category',
                        ticks: {
                            autoSkip: true,
                            maxRotation: 0,
                            callback: (value, index, ticks) => {
                                let date = labels[index];
                                return ticks.length > 30 ? date.split(' ')[0] : date;
                            }
                        }
                    }
                }
            }
        });

    } catch (error) {
        console.error("Error loading market data:", error);
    }
}

function validateDate() {
    let startDate = document.getElementById("startDate").value;
    let endDate = document.getElementById("endDate").value;

    if (startDate && endDate && new Date(startDate) > new Date(endDate)) {
        alert("End Date must be after Start Date.");
        document.getElementById("endDate").value = "";
    }
}

document.addEventListener("DOMContentLoaded", fetchMarketData);