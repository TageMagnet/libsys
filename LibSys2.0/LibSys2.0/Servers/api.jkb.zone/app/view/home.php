<!DOCTYPE html>
<html>

<head>
  <title>
    api.jkb.zone
  </title>
  <meta http-equiv="content-type" content="text/html; charset=utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="/assets/css/common.css"/>
  <style>
    .rainbow:nth-child(1) {
      color: red;
    }

    .rainbow:nth-child(2) {
      color: blue;
    }

    .rainbow:nth-child(3) {
      color: teal;
    }

    .rainbow:nth-child(4) {
      color: purple;
    }

    .rainbow:nth-child(5) {
      color: orange;
    }

    .rainbow:nth-child(6) {
      color: brown;
    }

    .rainbow:nth-child(7) {
      color: #f9f9f9;
      text-shadow: -1px -1px 0 #000, 1px -1px 0 #000, -1px 1px 0 #000, 1px 1px 0 #000;
    }

    .rainbow:nth-child(8) {
      color: yellow;
      text-shadow: -1px -1px 0 #000, 1px -1px 0 #000, -1px 1px 0 #000, 1px 1px 0 #000;
    }

    .params {
      color:var(--light-gray);
    }
  </style>
</head>

<body>
  <div class="lc">
    <div>
      <?php
      echo "<i>{$req->host}</i>{$directories}<span class=\"params\">{$query}</span>";
      echo "<br>";
      ?>
    </div>
  </div>
</body>

</html>