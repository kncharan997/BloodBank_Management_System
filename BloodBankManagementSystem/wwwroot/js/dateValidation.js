
<script>
    document.addEventListener("DOMContentLoaded", function () {
        let startDateInput = document.getElementById("campStartDate");
    let endDateInput = document.getElementById("campEndDate");

    startDateInput.addEventListener("change", function () {
        let today = new Date().toISOString().split("T")[0];
    if (startDateInput.value < today) {
        alert("Start Date cannot be in the past.");
    startDateInput.value = "";
        }
    endDateInput.min = startDateInput.value; // Update end date min value
    });

    endDateInput.addEventListener("change", function () {
        if (endDateInput.value < startDateInput.value) {
        alert("End Date cannot be before Start Date.");
    endDateInput.value = startDateInput.value;
        }
    });
});
</script>