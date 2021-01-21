<?php
namespace SampleProject\Protocol\Context;

abstract class CurrencyTypes {
    const EUR = 1;
    const USD = 2;

    const MapToName = array(
        1 => "EUR",
        2 => "USD"
    );
}
